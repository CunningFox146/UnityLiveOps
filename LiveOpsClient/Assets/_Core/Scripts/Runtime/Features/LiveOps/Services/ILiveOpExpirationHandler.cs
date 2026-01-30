using App.Runtime.Features.LiveOps.Models;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Services
{
    public interface ILiveOpExpirationHandler
    {
        bool IsExpired(LiveOpState state);
        UniTask UnloadIfExpired(LiveOpState state);
    }
}
