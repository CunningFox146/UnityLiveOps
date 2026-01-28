using System;
using System.Collections.Generic;
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
using UnityEngine.Pool;
using ZLinq;

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

                if (Data.Id == activeCalendarId)
                {
                    _logger.Info($"Using cached calendar id: {activeCalendarId}", LoggerTag.LiveOps);
                    return;
                }

                var calendarDto = await _apiService.GetCalendar(token);
                Data.UpdateFromDto(calendarDto, _timeService);
                await _repository.UpdateAsync(Data, token);
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
            var activeEvents = HashSetPool<FeatureType>.Get();
            try
            {
                foreach (var liveOp in Data.Events)
                {
                    if (_userStateService.CurrentLevel < liveOp.EntryLevel)
                        return;

                    var lookBackTime = CurrentTime - liveOp.Duration;
                    var occurrenceStart = liveOp.Schedule.GetNextOccurrence(lookBackTime);
                    var occurrenceEnd = occurrenceStart + liveOp.Duration;
                    var isRunning = occurrenceEnd > CurrentTime && occurrenceStart < CurrentTime;

                    var state = new LiveOpState(liveOp.Type, occurrenceStart, occurrenceEnd);
                    if (isRunning)
                    {
                        activeEvents.Add(liveOp.Type);
                        Data.RecordEvent(state);
                        StartEvent(state);
                    }
                    else
                        ScheduleEvent(state, token).Forget();
                }

                _repository.Update(Data);
            }
            catch (Exception exception)
            {
                _logger.Error("Failed to schedule events", exception, LoggerTag.LiveOps);
            }
            finally
            {
                StartExpiredEvents(activeEvents);
                HashSetPool<FeatureType>.Release(activeEvents);
            }
        }

        private void StartExpiredEvents(HashSet<FeatureType> activeEvents)
        {
            var expired = Data.SeenEvents.AsValueEnumerable()
                .Where(e => !activeEvents.Contains(e.Type) && e.EndTime < CurrentTime);

            foreach (var expiredEvent in expired)
            {
                StartEvent(expiredEvent);
            }
        }

        private async UniTaskVoid ScheduleEvent(LiveOpState state, CancellationToken token)
        {
            try
            {
                await UniTask.Delay(state.StartTime - CurrentTime, cancellationToken: token);
                _featureService.StopFeature(state.Type);
                Data.RecordEvent(state);
                StartEvent(state);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.Error($"Failed to schedule event {state.Type}", ex, LoggerTag.LiveOps);
            }
        }

        private void StartEvent(LiveOpState state)
        {
            _featureService.StartFeature(state.Type, builder =>
            {
                new ClickerLiveOpInstaller(state).Install(builder);
            });
        }
    }
}