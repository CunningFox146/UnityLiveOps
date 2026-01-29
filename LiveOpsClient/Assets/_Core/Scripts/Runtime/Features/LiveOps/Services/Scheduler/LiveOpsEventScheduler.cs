using System;
using System.Threading;
using App.Runtime.Features.Common.Services;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services.Calendar;
using App.Runtime.Features.UserState.Services;
using App.Shared.Logger;
using App.Shared.Time;
using Cysharp.Threading.Tasks;
using ZLinq;

namespace App.Runtime.Features.LiveOps.Services.Scheduler
{
    public class LiveOpsEventScheduler : ILiveOpsEventScheduler
    {
        private readonly ILiveOpsCalendarHandler _calendarHandler;
        private readonly ITimeService _timeService;
        private readonly IFeatureService _featureService;
        private readonly IUserStateService _userStateService;
        private readonly ILogger _logger;

        private LiveOpsCalendar Calendar => _calendarHandler.Calendar;
        private DateTime ServerTime => _timeService.Now - Calendar.TimeDifference;

        public LiveOpsEventScheduler(
            ILiveOpsCalendarHandler calendarHandler,
            ITimeService timeService,
            IFeatureService featureService,
            IUserStateService userStateService,
            ILogger logger)
        {
            _calendarHandler = calendarHandler;
            _timeService = timeService;
            _featureService = featureService;
            _userStateService = userStateService;
            _logger = logger;
        }

        public void ScheduleAllEvents(CancellationToken token)
        {
            try
            {
                ProcessCalendarEvents(token);
                StartPreviouslySeenExpiredEvents();
            }
            catch (Exception exception)
            {
                _logger.Error("Failed to schedule live ops events", exception, LoggerTag.LiveOps);
            }
        }

        private void ProcessCalendarEvents(CancellationToken token)
        {
            foreach (var liveOpEvent in Calendar.Events)
            {
                if (!IsPlayerEligibleForEvent(liveOpEvent))
                    continue;

                var eventState = CalculateEventState(liveOpEvent);
                if (IsEventCurrentlyActive(eventState))
                {
                    Calendar.RecordEvent(eventState);
                    StartLiveOp(eventState);
                }
                else
                {
                    ScheduleEventStartAsync(eventState, token).Forget(_logger.LogUniTask);
                }
            }
        }

        private LiveOpState CalculateEventState(LiveOpEvent liveOpEvent)
        {
            var lookBackTime = ServerTime - liveOpEvent.Duration;
            var occurrenceStart = liveOpEvent.Schedule.GetNextOccurrence(lookBackTime);
            var occurrenceEnd = occurrenceStart + liveOpEvent.Duration;

            return new LiveOpState(liveOpEvent.Type, occurrenceStart, occurrenceEnd);
        }

        private async UniTask ScheduleEventStartAsync(LiveOpState eventState, CancellationToken token)
        {
            try
            {
                var delay = eventState.StartTime - ServerTime;
                await UniTask.Delay(delay, cancellationToken: token);

                _featureService.StopFeature(eventState.Type);
                Calendar.RecordEvent(eventState);

                StartLiveOp(eventState);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.Error($"Failed to start scheduled event: {eventState.Type}", ex, LoggerTag.LiveOps);
            }
        }

        private void StartPreviouslySeenExpiredEvents()
        {
            var expiredEvents = Calendar.SeenEvents
                .AsValueEnumerable()
                .Where(IsEventExpiredAndNotActive);

            foreach (var expiredEvent in expiredEvents)
                StartLiveOp(expiredEvent);
        }

        private void StartLiveOp(LiveOpState eventState)
        {
            var installer = LiveOpInstallersPerFeature.GetInstaller(eventState);
            _featureService.StartFeature(eventState.Type, installer);
        }
        
        private bool IsPlayerEligibleForEvent(LiveOpEvent liveOpEvent)
            => _userStateService.CurrentLevel >= liveOpEvent.EntryLevel;
        
        private bool IsEventCurrentlyActive(LiveOpState eventState)
            => eventState.StartTime < ServerTime && eventState.EndTime > ServerTime;

        private bool IsEventExpiredAndNotActive(LiveOpState eventState)
            => !_featureService.IsFeatureActive(eventState.Type) && eventState.EndTime < ServerTime;
    }
}
