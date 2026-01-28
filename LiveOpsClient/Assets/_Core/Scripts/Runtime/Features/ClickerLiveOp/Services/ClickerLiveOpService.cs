using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.LiveOps.Models;
using App.Shared.Repository;
using App.Shared.Time;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.ClickerLiveOp.Services
{
    public class ClickerLiveOpService
    {
        private readonly IRepository<ClickerLiveOpState> _repository;
        private readonly LiveOpEvent _liveOpEvent;
        private readonly ITimeService _timeService;
        public int Progress => State.Progress;
        private ClickerLiveOpState State => _repository.Value;
        
        public ClickerLiveOpService(IRepository<ClickerLiveOpState> repository, LiveOpEvent liveOpEvent, ITimeService timeService)
        {
            _repository = repository;
            _liveOpEvent = liveOpEvent;
            _timeService = timeService;
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
                
            }
        }

        public void IncrementProgress()
        {
            if (_liveOpEvent.IsExpired(_timeService))
                return;
            
            State.Progress++;
            _repository.Update(State);
        }
    }
}