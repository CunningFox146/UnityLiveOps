using System;
using System.Threading;
using App.Runtime.Features.LiveOps.Models;
using App.Shared.Logger;
using App.Shared.Repository;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Services.Lifecycle
{
    public class LiveOpDataLifecycle<TData> : ILiveOpDataLifecycle
        where TData : class, ILiveOpData
    {
        private readonly IRepository<TData> _repository;
        private readonly LiveOpState _state;
        private readonly ILogger _logger;
        private TData Data => _repository.Value;

        public LiveOpDataLifecycle(IRepository<TData> repository, LiveOpState state, ILogger logger)
        {
            _repository = repository;
            _state = state;
            _logger = logger;
        }

        public async UniTask RestoreAndValidateData(CancellationToken token)
        {
            try
            {
                await _repository.RestoreFeatureData(token);
                
                if (Data.EventStartTime != _state.StartTime)
                    ResetData();
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Failed to restore LiveOp data", exception, LoggerTag.LiveOps);
            }
        }

        private void ResetData()
        {
            Data.Clear();
            Data.EventStartTime = _state.StartTime;
            _repository.Update(Data);
        }
    }
}
