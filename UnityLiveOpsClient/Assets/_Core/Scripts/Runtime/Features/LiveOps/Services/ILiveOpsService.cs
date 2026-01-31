using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Services
{
    public interface ILiveOpsService
    {
        UniTask Initialize(CancellationToken token);
    }
}