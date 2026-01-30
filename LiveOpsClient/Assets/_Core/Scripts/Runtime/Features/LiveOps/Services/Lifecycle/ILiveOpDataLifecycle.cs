using System.Threading;
using App.Runtime.Features.LiveOps.Models;
using App.Shared.Repository;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Services.Lifecycle
{
    public interface ILiveOpDataLifecycle
    {
        UniTask RestoreAndValidateData<TData>(
            IRepository<TData> repository, 
            LiveOpState state,
            CancellationToken token) where TData : ILiveOpData;
    }
}
