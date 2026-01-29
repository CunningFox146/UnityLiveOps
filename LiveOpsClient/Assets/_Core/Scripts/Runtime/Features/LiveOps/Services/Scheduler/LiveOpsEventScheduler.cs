using System;
using System.Collections.Generic;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.Common;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.UserState.Services;
using App.Shared.Logger;
using App.Shared.Time;
using Cysharp.Threading.Tasks;
using UnityEngine.Pool;
using ZLinq;

namespace App.Runtime.Features.LiveOps.Services
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
            var activeEventTypes = HashSetPool<FeatureType>.Get();
            try
            {
                ProcessCalendarEvents(activeEventTypes, token);
                StartPreviouslySeenExpiredEvents(activeEventTypes);
            }
            catch (Exception exception)
            {
                _logger.Error("Failed to schedule live ops events", exception, LoggerTag.LiveOps);
            }
            finally
            {
                HashSetPool<FeatureType>.Release(activeEventTypes);
            }
        }

        private void ProcessCalendarEvents(HashSet<FeatureType> activeEventTypes, CancellationToken token)
        {
            foreach (var liveOpEvent in Calendar.Events)
            {
                if (!IsPlayerEligibleForEvent(liveOpEvent))
                    continue;

                var eventState = CalculateEventState(liveOpEvent);

                if (IsEventCurrentlyActive(eventState))
                {
                    StartActiveEvent(eventState, activeEventTypes);
                }
                else
                {
                    ScheduleFutureEvent(eventState, token);
                }
            }
        }

        private bool IsPlayerEligibleForEvent(LiveOpEvent liveOpEvent)
        {
            return _userStateService.CurrentLevel >= liveOpEvent.EntryLevel;
        }

        private LiveOpState CalculateEventState(LiveOpEvent liveOpEvent)
        {
            var lookBackTime = ServerTime - liveOpEvent.Duration;
            var occurrenceStart = liveOpEvent.Schedule.GetNextOccurrence(lookBackTime);
            var occurrenceEnd = occurrenceStart + liveOpEvent.Duration;

            return new LiveOpState(liveOpEvent.Type, occurrenceStart, occurrenceEnd);
        }

        private bool IsEventCurrentlyActive(LiveOpState eventState)
        {
            return eventState.StartTime < ServerTime && eventState.EndTime > ServerTime;
        }

        private void StartActiveEvent(LiveOpState eventState, HashSet<FeatureType> activeEventTypes)
        {
            activeEventTypes.Add(eventState.Type);
            Calendar.RecordEvent(eventState);

            var installer = LiveOpInstallersPerFeature.GetInstaller(eventState);
            _featureService.StartFeature(eventState.Type, installer);
        }

        private void ScheduleFutureEvent(LiveOpState eventState, CancellationToken token)
        {
            ScheduleEventStartAsync(eventState, token).Forget();
        }

        private async UniTaskVoid ScheduleEventStartAsync(LiveOpState eventState, CancellationToken token)
        {
            try
            {
                var delay = eventState.StartTime - ServerTime;
                await UniTask.Delay(delay, cancellationToken: token);

                _featureService.StopFeature(eventState.Type);
                Calendar.RecordEvent(eventState);

                var installer = LiveOpInstallersPerFeature.GetInstaller(eventState);
                _featureService.StartFeature(eventState.Type, installer);
            }
            catch (OperationCanceledException)
            {
                // Expected when app closes or token is cancelled
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to start scheduled event: {eventState.Type}", ex, LoggerTag.LiveOps);
            }
        }

        private void StartPreviouslySeenExpiredEvents(HashSet<FeatureType> activeEventTypes)
        {
            var expiredEvents = Calendar.SeenEvents.AsValueEnumerable()
                .Where(e => IsEventExpiredAndNotActive(e, activeEventTypes));

            foreach (var expiredEvent in expiredEvents)
            {
                var installer = LiveOpInstallersPerFeature.GetInstaller(expiredEvent);
                _featureService.StartFeature(expiredEvent.Type, installer);
            }
        }

        private bool IsEventExpiredAndNotActive(LiveOpState eventState, HashSet<FeatureType> activeEventTypes)
        {
            return !activeEventTypes.Contains(eventState.Type) && eventState.EndTime < ServerTime;
        }
    }
}
