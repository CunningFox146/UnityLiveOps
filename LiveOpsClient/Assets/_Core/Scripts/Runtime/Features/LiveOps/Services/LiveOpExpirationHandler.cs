using App.Runtime.Features.Common.Services;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services.Calendar;
using App.Shared.Time;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Services
{
    public class LiveOpExpirationHandler : ILiveOpExpirationHandler
    {
        private readonly ITimeService _timeService;
        private readonly IFeatureService _featureService;
        private readonly ILiveOpsCalendarHandler _calendarHandler;

        public LiveOpExpirationHandler(
            ITimeService timeService,
            IFeatureService featureService,
            ILiveOpsCalendarHandler calendarHandler)
        {
            _timeService = timeService;
            _featureService = featureService;
            _calendarHandler = calendarHandler;
        }

        public bool IsExpired(LiveOpState state)
        {
            return state.IsExpired(_timeService);
        }

        public async UniTask UnloadIfExpired(LiveOpState state)
        {
            if (!IsExpired(state))
                return;
            
            _featureService.StopFeature(state.Type);
            _calendarHandler.RemoveSeenEvent(state);
            await UniTask.CompletedTask;
        }
    }
}
