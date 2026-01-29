using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.KeyCollectLiveOp.Services
{
    public interface IKeyCollectLiveOpService
    {
        int KeysCollected { get; }
        UniTask Initialize(CancellationToken token);
        void TryUnloadFeatureIfExpired();
    }
}
