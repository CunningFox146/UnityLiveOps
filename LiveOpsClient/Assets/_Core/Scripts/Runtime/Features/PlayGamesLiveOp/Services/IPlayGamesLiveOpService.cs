using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.PlayGamesLiveOp.Services
{
    public interface IPlayGamesLiveOpService
    {
        int GamesPlayed { get; }
        UniTask Initialize(CancellationToken token);
        void TryUnloadFeatureIfExpired();
    }
}
