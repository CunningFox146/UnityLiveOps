using System;
using System.Threading;
using App.Runtime.Features.Common;
using App.Runtime.Features.LiveOps.Model;
using App.Runtime.Features.LiveOps.Services;
using App.Shared.Repository;
using App.Shared.Time;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Service
{
    public class LiveOpsService
    {
        private readonly IRepository<LiveOpsCalendar> _repository;
        private readonly ILiveOpsApiService _apiService;
        private readonly ITimeService _timeService;
        private readonly IFeatureService _featureService;
        private LiveOpsCalendar Data => _repository.Value;

        public LiveOpsService(IRepository<LiveOpsCalendar> repository, ILiveOpsApiService apiService, ITimeService timeService, IFeatureService featureService)
        {
            _repository = repository;
            _apiService = apiService;
            _timeService = timeService;
            _featureService = featureService;
        }
        
        public async UniTask Initialize(CancellationToken token)
        {
            var activeCalendarId = await _apiService.GetCalendarId(token);
            await _repository.RestoreFeatureData(token);

            if (activeCalendarId == Guid.Empty || Data.Id != activeCalendarId)
            {
                var calendarDto = await _apiService.GetCalendar(token);
                var calendar = LiveOpsCalendar.CreateFromDto(calendarDto, _timeService);
                await _repository.UpdateAsync(calendar, token);
            }
        }

        public void ScheduleLiveOps(CancellationToken token)
        {
            foreach (var liveOp in Data.Events)
            {
                var currentTime = _timeService.Now - Data.TimeDifference;
                var nextOccurrence = liveOp.Schedule.GetNextOccurrence(currentTime);
                var delay = nextOccurrence - currentTime;
                var occurrenceId = GetOccurrenceId(liveOp.Id, nextOccurrence);

                if (delay > TimeSpan.Zero && !Data.SeenEvents.Contains(occurrenceId))
                {
                    ScheduleEvent(liveOp, occurrenceId, delay, token);
                }
            }
        }

        private void ScheduleEvent(LiveOpEvent liveOp, int occurrenceId, TimeSpan delay, CancellationToken token)
        {
            // Create new scope
            // in scope:
            // 1. Load assets
            // 2. Register icon

            _featureService.StartFeature(FeatureType.LiveOpClicker, LiveOpInstaller.Default, token).Forget();
        }

        private static int GetOccurrenceId(Guid eventId, DateTime occurrenceTime)
            => HashCode.Combine(eventId, occurrenceTime.Ticks);
    }
}