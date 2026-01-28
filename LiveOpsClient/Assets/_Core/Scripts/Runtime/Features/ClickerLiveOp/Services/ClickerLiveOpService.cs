using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Shared.Logger;
using App.Shared.Repository;
using App.Shared.Time;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.ClickerLiveOp.Services
{
    public class ClickerLiveOpService : IClickerLiveOpService
    {
        private readonly IRepository<ClickerLiveOpData> _repository;
        private readonly LiveOpState _liveOpEvent;
        private readonly ITimeService _timeService;
        private readonly ILogger _logger;
        public int Progress => Data.Progress;
        private ClickerLiveOpData Data => _repository.Value;
        
        public ClickerLiveOpService(IRepository<ClickerLiveOpData> repository, LiveOpState liveOpEvent, ITimeService timeService, ILogger logger)
        {
            _repository = repository;
            _liveOpEvent = liveOpEvent;
            _timeService = timeService;
            _logger = logger;
        }

        public async UniTask Initialize(CancellationToken token)
        {
            try
            {
                await _repository.RestoreFeatureData(token);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                _logger.Error("Failed to initialize Clicker LiveOp Service", exception, LoggerTag.LiveOps);
            }
        }

        public void IncrementProgress()
        {
            if (_liveOpEvent.IsExpired(_timeService))
                return;
            
            Data.Progress++;
            _repository.Update(Data);
        }
    }
}