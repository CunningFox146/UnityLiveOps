using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.ClickerLiveOp.Services
{
    public interface IClickerLiveOpService
    {
        int Progress { get; }
        UniTask Initialize(CancellationToken token);
        void IncrementProgress();
    }
}