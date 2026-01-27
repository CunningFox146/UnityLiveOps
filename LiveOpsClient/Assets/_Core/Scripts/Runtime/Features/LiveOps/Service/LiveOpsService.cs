using System;
using System.Threading;
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
        private LiveOpsCalendar Data => _repository.Value;

        public LiveOpsService(IRepository<LiveOpsCalendar> repository, ILiveOpsApiService apiService, ITimeService timeService)
        {
            _repository = repository;
            _apiService = apiService;
            _timeService = timeService;
        }
        
        public async UniTask Initialize(CancellationToken token)
        {
            var activeCalendarId = await _apiService.GetCalendarId(token);
            await _repository.RestoreFeatureData(token);

            if (activeCalendarId == Guid.Empty || Data.Id != activeCalendarId)
            {
                var calendarDto = await _apiService.GetCalendar(token);
                var calendar = LiveOpsCalendar.CreateFromDto(calendarDto, _timeService);
                _repository.Update(calendar);
            }
        } 
    }
}