using System;
using System.Threading;
using App.Runtime.Features.LiveOps.Models;
using App.Shared.Logger;
using App.Shared.Repository;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Services.Lifecycle
{
    public class LiveOpDataLifecycle : ILiveOpDataLifecycle
    {
        private readonly ILogger _logger;

        public LiveOpDataLifecycle(ILogger logger)
        {
            _logger = logger;
        }

        public async UniTask RestoreAndValidateData<TData>(
            IRepository<TData> repository, 
            LiveOpState state, 
            CancellationToken token) where TData : ILiveOpData
        {
            try
            {
                await repository.RestoreFeatureData(token);
                
                var data = repository.Value;
                if (data.EventStartTime != state.StartTime)
                {
                    repository.Clear();
                    data.EventStartTime = state.StartTime;
                    repository.Update(data);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Failed to restore LiveOp data", exception, LoggerTag.LiveOps);
            }
        }
    }
}
