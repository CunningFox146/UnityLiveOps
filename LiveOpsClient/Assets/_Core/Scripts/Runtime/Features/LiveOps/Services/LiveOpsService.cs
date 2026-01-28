using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp;
using App.Runtime.Features.Common;
using App.Runtime.Features.LiveOps.Api;
using App.Runtime.Features.LiveOps.Models;
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
        private readonly ILogger _logger;
        private LiveOpsCalendar Data => _repository.Value;

        public LiveOpsService(IRepository<LiveOpsCalendar> repository, ILiveOpsApiService apiService, ITimeService timeService, IFeatureService featureService, ILogger logger)
        {
            _repository = repository;
            _apiService = apiService;
            _timeService = timeService;
            _featureService = featureService;
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
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.Error("Failed to initialize LiveOps service.", ex, LoggerTag.LiveOps);
            }
        }

        public void ScheduleLiveOps(CancellationToken token)
        {
            foreach (var liveOp in Data.Events)
            {
                try
                {
                    var currentTime = _timeService.Now - Data.TimeDifference;
                    var nextOccurrence = liveOp.Schedule.GetNextOccurrence(currentTime);
                    var startsIn = currentTime - nextOccurrence;
                    var endsIn = startsIn + liveOp.Duration;
                    if (endsIn.Ticks > 0)
                    {
                        ScheduleEvent(liveOp, token);
                    }

                }
                catch (Exception exception)
                {
                    _logger.Error($"Failed to schedule event {liveOp.Id}", exception, LoggerTag.LiveOps);
                }
            }
        }

        private void ScheduleEvent(LiveOpEvent liveOp, CancellationToken token)
        {
            _featureService.StartFeature(liveOp.Type, builder =>
            {
                builder.RegisterInstance(liveOp.Type);
                new ClickerLiveOpInstaller(liveOp).Install(builder);
            });
        }

        private static int GetOccurrenceId(Guid eventId, DateTime occurrenceTime)
            => HashCode.Combine(eventId, occurrenceTime.Ticks);
    }
}