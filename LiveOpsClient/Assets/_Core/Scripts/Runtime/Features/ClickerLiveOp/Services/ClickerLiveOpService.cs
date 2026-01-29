using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.Common.Services;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services.Calendar;
using App.Shared.Logger;
using App.Shared.Repository;
using App.Shared.Time;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.ClickerLiveOp.Services
{
    public class ClickerLiveOpService : IClickerLiveOpService
    {
        private readonly IRepository<ClickerLiveOpData> _repository;
        private readonly LiveOpState _state;
        private readonly ITimeService _timeService;
        private readonly IFeatureService _featureService;
        private readonly ILiveOpsCalendarHandler _calendarHandler;
        private readonly ILogger _logger;
        public int Progress => Data.Progress;
        private ClickerLiveOpData Data => _repository.Value;

        public ClickerLiveOpService(IRepository<ClickerLiveOpData> repository, LiveOpState state,
            ITimeService timeService, IFeatureService featureService, ILiveOpsCalendarHandler calendarHandler,
            ILogger logger)
        {
            _repository = repository;
            _state = state;
            _timeService = timeService;
            _featureService = featureService;
            _calendarHandler = calendarHandler;
            _logger = logger;
        }

        public async UniTask Initialize(CancellationToken token)
        {
            try
            {
                await _repository.RestoreFeatureData(token);
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Failed to initialize Clicker LiveOp Service", exception, LoggerTag.LiveOps);
            }
        }

        public void IncrementProgress()
        {
            if (_state.IsExpired(_timeService))
                return;
            
            Data.Progress++;
            _repository.Update(Data);
        }

        public void TryUnloadFeatureIfExpired()
        {
            if (!_state.IsExpired(_timeService))
                return;
            
            _featureService.StopFeature(_state.Type);
            _calendarHandler.RemoveSeenEvent(_state);
            _repository.Clear();
        }
    }
}