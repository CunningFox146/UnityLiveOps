using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Services.Lifecycle
{
    public interface ILiveOpDataLifecycle
    {
        UniTask RestoreAndValidateData(CancellationToken token);
    }
}
