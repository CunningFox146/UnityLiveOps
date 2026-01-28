using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.Common;
using App.Runtime.Features.LiveOps.Api;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.UserState.Services;
using App.Shared.Logger;
using App.Shared.Repository;
using App.Shared.Time;
using Cysharp.Threading.Tasks;
using UnityEngine.LightTransport;
using VContainer;

namespace App.Runtime.Features.LiveOps.Services
{
    public class LiveOpsService : ILiveOpsService
    {
        private readonly IRepository<LiveOpsCalendar> _repository;
        private readonly ILiveOpsApiService _apiService;
        private readonly ITimeService _timeService;
        private readonly IFeatureService _featureService;
        private readonly IUserStateService _userStateService;
        private readonly ILogger _logger;
        private LiveOpsCalendar Data => _repository.Value;
        private DateTime CurrentTime => _timeService.Now - Data.TimeDifference;

        public LiveOpsService(IRepository<LiveOpsCalendar> repository, ILiveOpsApiService apiService,
            ITimeService timeService, IFeatureService featureService, IUserStateService userStateService,
            ILogger logger)
        {
            _repository = repository;
            _apiService = apiService;
            _timeService = timeService;
            _featureService = featureService;
            _userStateService = userStateService;
            _logger = logger;
        }

        public async UniTask Initialize(CancellationToken token)
        {
            await LoadCalendar(token);
            ScheduleLiveOps(token);
        }

        private async UniTask LoadCalendar(CancellationToken token)
        {
            try
            {
                await _repository.RestoreFeatureData(token);
                var activeCalendarId = await _apiService.GetCalendarId(token);

                if (activeCalendarId == Guid.Empty || Data.Id == activeCalendarId)
                {
                    _logger.Info("Using cached calendar id: " + activeCalendarId, LoggerTag.LiveOps);
                    return;
                }

                var calendarDto = await _apiService.GetCalendar(token);
                var calendar = LiveOpsCalendar.CreateFromDto(calendarDto, _timeService);
                await _repository.UpdateAsync(calendar, token);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to initialize LiveOps service.", ex, LoggerTag.LiveOps);
            }
        }

        public void ScheduleLiveOps(CancellationToken token)
        {
            foreach (var liveOp in Data.Events)
                try
                {
                    if (_userStateService.CurrentLevel < liveOp.EntryLevel)
                        return;
                    
                    var lookBackTime = CurrentTime - liveOp.Duration;
                    var occurrenceStart = liveOp.Schedule.GetNextOccurrence(lookBackTime);
                    var occurrenceEnd = occurrenceStart + liveOp.Duration;
                    var isRunning = occurrenceEnd > CurrentTime && occurrenceStart < CurrentTime;

                    if (isRunning)
                        StartEvent(liveOp, occurrenceStart, occurrenceEnd);
                    else
                        ScheduleEvent(liveOp, occurrenceStart, occurrenceEnd, token).Forget();
                }
                catch (Exception exception)
                {
                    _logger.Error($"Failed to schedule event {liveOp.Id}", exception, LoggerTag.LiveOps);
                }
        }

        private async UniTaskVoid ScheduleEvent(LiveOpEvent liveOp, DateTime occurrenceStart, DateTime occurrenceEnd,
            CancellationToken token)
        {
            try
            {
                await UniTask.Delay(occurrenceStart - CurrentTime, cancellationToken: token);
                StartEvent(liveOp, occurrenceStart, occurrenceEnd);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.Error($"Failed to schedule event {liveOp.Type}", ex, LoggerTag.LiveOps);
            }
        }

        private void StartEvent(LiveOpEvent liveOp, DateTime startsAt, DateTime endsAt)
        {
            // Will be saved to seen events to be shown as expired
            var initialState = new LiveOpState
            {
                FeatureType = liveOp.Type,
                StartTime = startsAt,
                EndTime = endsAt,
            };
            
            _featureService.StartFeature(liveOp.Type, builder =>
            {
                new ClickerLiveOpInstaller(initialState).Install(builder);
            });
        }

        private static int GetOccurrenceId(Guid eventId, DateTime occurrenceTime)
        {
            return HashCode.Combine(eventId, occurrenceTime.Ticks);
        }
    }
}