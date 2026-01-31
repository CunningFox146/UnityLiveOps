using System;
using System.Collections.Generic;
using App.Runtime.Features.Common.Models;
using App.Runtime.Features.Common.Services;
using App.Runtime.Features.LiveOps.Factories;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services.Calendar;
using App.Runtime.Features.LiveOps.Services.Scheduler;
using App.Runtime.Features.UserState.Services;
using App.Shared.Time;
using CunningFox.LiveOps.Models;
using NSubstitute;
using NUnit.Framework;
using VContainer.Unity;
using ILogger = App.Shared.Logger.ILogger;

namespace App.Tests
{
    public class LiveOpsEventSchedulerTests : UnitTestsBase
    {
        private static readonly DateTime DefaultTime = new(2026, 1, 30, 12, 0, 0);
        private const int DefaultPlayerLevel = 10;

        private ILiveOpsCalendarHandler _calendarHandler;
        private ITimeService _timeService;
        private IFeatureService _featureService;
        private IUserStateService _userStateService;
        private ILiveOpInstallerFactory _installerFactory;
        private ILogger _logger;
        private IInstaller _installer;
        private LiveOpsEventScheduler _service;
        private LiveOpsCalendar _calendar;

        protected override void OnSetUp()
        {
            _calendarHandler = Substitute.For<ILiveOpsCalendarHandler>();
            _timeService = Substitute.For<ITimeService>();
            _featureService = Substitute.For<IFeatureService>();
            _userStateService = Substitute.For<IUserStateService>();
            _installerFactory = Substitute.For<ILiveOpInstallerFactory>();
            _logger = Substitute.For<ILogger>();
            _installer = Substitute.For<IInstaller>();

            _calendar = LiveOpsCalendar.Empty;
            _calendarHandler.Calendar.Returns(_calendar);

            _timeService.Now.Returns(DefaultTime);
            _userStateService.CurrentLevel.Returns(DefaultPlayerLevel);
            _installerFactory.CreateInstaller(Arg.Any<LiveOpState>()).Returns(_installer);

            _service = new LiveOpsEventScheduler(
                _calendarHandler,
                _timeService,
                _featureService,
                _userStateService,
                _installerFactory,
                _logger);
        }

        protected override void OnTearDown()
        {
            _service?.Dispose();
            _service = null;
        }

        private void SetupCalendarWithEvents(params LiveOpDto[] events)
        {
            var now = _timeService.Now;
            var dto = new LiveOpsCalendarDto("cal1", now.Ticks, new List<LiveOpDto>(events));
            _calendar.UpdateFromDto(dto, _timeService);
        }

        private void SetupEmptyCalendar()
        {
            var now = _timeService.Now;
            var dto = new LiveOpsCalendarDto("cal1", now.Ticks, new List<LiveOpDto>());
            _calendar.UpdateFromDto(dto, _timeService);
        }

        private static LiveOpDto CreateEvent(
            string id = "event1",
            string schedule = "0 11 * * *",
            int durationHours = 2,
            string eventName = "ClickerLiveOp",
            int entryLevel = 1)
            => new(id, schedule, TimeSpan.FromHours(durationHours), eventName, entryLevel);

        [Test]
        public void ScheduleAllEvents_WithNoEvents_DoesNotStartAnyFeatures()
        {
            _service.ScheduleAllEvents();

            _featureService.DidNotReceive().StartFeature(Arg.Any<FeatureType>(), Arg.Any<IInstaller>());
        }

        [Test]
        public void ScheduleAllEvents_WithActiveEvent_StartsFeature()
        {
            SetupCalendarWithEvents(CreateEvent());

            _service.ScheduleAllEvents();

            _featureService.Received(1).StartFeature(FeatureType.ClickerLiveOp, _installer);
        }

        [Test]
        public void ScheduleAllEvents_WithActiveEvent_RecordsEventInCalendar()
        {
            SetupCalendarWithEvents(CreateEvent());

            _service.ScheduleAllEvents();

            Assert.IsTrue(_calendar.SeenEvents.ContainsKey(FeatureType.ClickerLiveOp));
        }

        [Test]
        public void ScheduleAllEvents_PlayerLevelBelowEntryLevel_DoesNotStartFeature()
        {
            _userStateService.CurrentLevel.Returns(5);
            SetupCalendarWithEvents(CreateEvent(entryLevel: 10));

            _service.ScheduleAllEvents();

            _featureService.DidNotReceive().StartFeature(Arg.Any<FeatureType>(), Arg.Any<IInstaller>());
        }

        [Test]
        public void ScheduleAllEvents_PlayerLevelEqualsEntryLevel_StartsFeature()
        {
            SetupCalendarWithEvents(CreateEvent(entryLevel: 10));

            _service.ScheduleAllEvents();

            _featureService.Received(1).StartFeature(FeatureType.ClickerLiveOp, _installer);
        }

        [Test]
        public void ScheduleAllEvents_PlayerLevelAboveEntryLevel_StartsFeature()
        {
            _userStateService.CurrentLevel.Returns(20);
            SetupCalendarWithEvents(CreateEvent(entryLevel: 10));

            _service.ScheduleAllEvents();

            _featureService.Received(1).StartFeature(FeatureType.ClickerLiveOp, _installer);
        }

        [Test]
        public void ScheduleAllEvents_WithMultipleActiveEvents_StartsAllEligibleFeatures()
        {
            SetupCalendarWithEvents(
                CreateEvent("event1", eventName: "ClickerLiveOp"),
                CreateEvent("event2", eventName: "KeyCollectLiveOp"),
                CreateEvent("event3", eventName: "PlayGamesLiveOp"));

            _service.ScheduleAllEvents();

            _featureService.Received(1).StartFeature(FeatureType.ClickerLiveOp, _installer);
            _featureService.Received(1).StartFeature(FeatureType.KeyCollectLiveOp, _installer);
            _featureService.Received(1).StartFeature(FeatureType.PlayGamesLiveOp, _installer);
        }

        [Test]
        public void ScheduleAllEvents_WithMixedEligibility_OnlyStartsEligibleFeatures()
        {
            _userStateService.CurrentLevel.Returns(5);
            SetupCalendarWithEvents(
                CreateEvent("event1", eventName: "ClickerLiveOp", entryLevel: 1),
                CreateEvent("event2", eventName: "KeyCollectLiveOp", entryLevel: 10),
                CreateEvent("event3", eventName: "PlayGamesLiveOp", entryLevel: 3));

            _service.ScheduleAllEvents();

            _featureService.Received(1).StartFeature(FeatureType.ClickerLiveOp, _installer);
            _featureService.DidNotReceive().StartFeature(FeatureType.KeyCollectLiveOp, _installer);
            _featureService.Received(1).StartFeature(FeatureType.PlayGamesLiveOp, _installer);
        }

        [Test]
        public void ScheduleAllEvents_WithExpiredSeenEvent_StartsExpiredFeature()
        {
            SetupEmptyCalendar();
            var now = _timeService.Now;
            var expiredState = new LiveOpState(FeatureType.ClickerLiveOp, now.AddHours(-3), now.AddHours(-1));
            _calendar.RecordEvent(expiredState);
            _featureService.IsFeatureActive(FeatureType.ClickerLiveOp).Returns(false);

            _service.ScheduleAllEvents();

            _featureService.Received(1).StartFeature(FeatureType.ClickerLiveOp, _installer);
        }

        [Test]
        public void ScheduleAllEvents_WithExpiredSeenEventAlreadyActive_DoesNotRestartFeature()
        {
            SetupEmptyCalendar();
            var now = _timeService.Now;
            var expiredState = new LiveOpState(FeatureType.ClickerLiveOp, now.AddHours(-3), now.AddHours(-1));
            _calendar.RecordEvent(expiredState);
            _featureService.IsFeatureActive(FeatureType.ClickerLiveOp).Returns(true);

            _service.ScheduleAllEvents();

            _featureService.DidNotReceive().StartFeature(Arg.Any<FeatureType>(), Arg.Any<IInstaller>());
        }

        [Test]
        public void ScheduleAllEvents_WithSeenEventNotYetExpired_DoesNotStartFromSeenEvents()
        {
            SetupEmptyCalendar();
            var now = _timeService.Now;
            var activeState = new LiveOpState(FeatureType.ClickerLiveOp, now.AddHours(-1), now.AddHours(1));
            _calendar.RecordEvent(activeState);
            _featureService.IsFeatureActive(FeatureType.ClickerLiveOp).Returns(false);

            _service.ScheduleAllEvents();

            _featureService.DidNotReceive().StartFeature(Arg.Any<FeatureType>(), Arg.Any<IInstaller>());
        }

        [Test]
        public void ScheduleAllEvents_WithActiveEvent_CreatesInstallerWithCorrectState()
        {
            SetupCalendarWithEvents(CreateEvent());
            LiveOpState capturedState = null;
            _installerFactory.CreateInstaller(Arg.Do<LiveOpState>(s => capturedState = s))
                .Returns(Substitute.For<IInstaller>());

            _service.ScheduleAllEvents();

            Assert.IsNotNull(capturedState);
            Assert.AreEqual(FeatureType.ClickerLiveOp, capturedState.Type);
            Assert.AreEqual(new DateTime(2026, 1, 30, 11, 0, 0), capturedState.StartTime);
            Assert.AreEqual(new DateTime(2026, 1, 30, 13, 0, 0), capturedState.EndTime);
        }

        [Test]
        public void Dispose_CancelsScheduledEvents()
        {
            var now = new DateTime(2026, 1, 30, 10, 0, 0);
            _timeService.Now.Returns(now);
            SetupCalendarWithEvents(CreateEvent(schedule: "0 12 * * *"));

            _service.ScheduleAllEvents();
            _service.Dispose();
            _service = null;

            _featureService.DidNotReceive().StartFeature(Arg.Any<FeatureType>(), Arg.Any<IInstaller>());
        }

        [Test]
        public void ScheduleAllEvents_EventNotYetStarted_DoesNotStartImmediately()
        {
            _timeService.Now.Returns(new DateTime(2026, 1, 30, 10, 0, 0));
            SetupCalendarWithEvents(CreateEvent(schedule: "0 14 * * *"));

            _service.ScheduleAllEvents();

            _featureService.DidNotReceive().StartFeature(Arg.Any<FeatureType>(), Arg.Any<IInstaller>());
        }

        [Test]
        public void ScheduleAllEvents_EventJustStarted_StartsFeature()
        {
            _timeService.Now.Returns(new DateTime(2026, 1, 30, 11, 30, 0));
            SetupCalendarWithEvents(CreateEvent());

            _service.ScheduleAllEvents();

            _featureService.Received(1).StartFeature(FeatureType.ClickerLiveOp, _installer);
        }

        [Test]
        public void ScheduleAllEvents_EventAboutToEnd_StartsFeature()
        {
            _timeService.Now.Returns(new DateTime(2026, 1, 30, 12, 59, 0));
            SetupCalendarWithEvents(CreateEvent());

            _service.ScheduleAllEvents();

            _featureService.Received(1).StartFeature(FeatureType.ClickerLiveOp, _installer);
        }

        [Test]
        public void ScheduleAllEvents_EventJustEnded_DoesNotStartFromCalendarEvents()
        {
            _timeService.Now.Returns(new DateTime(2026, 1, 30, 13, 1, 0));
            SetupCalendarWithEvents(CreateEvent());

            _service.ScheduleAllEvents();

            _featureService.DidNotReceive().StartFeature(Arg.Any<FeatureType>(), Arg.Any<IInstaller>());
        }
    }
}