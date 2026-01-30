using App.Runtime.Features.Common.Services;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services.Calendar;
using App.Shared.Repository;
using App.Shared.Time;

namespace App.Runtime.Features.LiveOps.Services
{
    public class LiveOpExpirationHandler<TData> : ILiveOpExpirationHandler
        where TData : ILiveOpData
    {
        private readonly ITimeService _timeService;
        private readonly IFeatureService _featureService;
        private readonly ILiveOpsCalendarHandler _calendarHandler;
        private readonly IRepository<TData> _repository;
        private readonly LiveOpState _state;
        
        public bool IsExpired => _state.IsExpired(_timeService);

        public LiveOpExpirationHandler(
            ITimeService timeService,
            IFeatureService featureService,
            ILiveOpsCalendarHandler calendarHandler,
            IRepository<TData> repository,
            LiveOpState state)
        {
            _timeService = timeService;
            _featureService = featureService;
            _calendarHandler = calendarHandler;
            _repository = repository;
            _state = state;
        }

        public void UnloadIfExpired()
        {
            if (!IsExpired)
                return;
            
            _calendarHandler.RemoveSeenEvent(_state);
            _repository.Clear();
            _featureService.StopFeature(_state.Type);
        }
    }
}
