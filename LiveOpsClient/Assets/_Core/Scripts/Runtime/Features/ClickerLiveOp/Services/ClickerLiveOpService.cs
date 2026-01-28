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
        private readonly IRepository<ClickerLiveOpData> _repository;
        private readonly ITimeService _timeService;
        public int Progress => Data.Progress;
        private ClickerLiveOpData Data => _repository.Value;
        
        public ClickerLiveOpService(IRepository<ClickerLiveOpData> repository, LiveOpEvent liveOpEvent, ITimeService timeService)
        {
            _repository = repository;
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
            Data.Progress++;
            _repository.Update(Data);
        }
    }
}