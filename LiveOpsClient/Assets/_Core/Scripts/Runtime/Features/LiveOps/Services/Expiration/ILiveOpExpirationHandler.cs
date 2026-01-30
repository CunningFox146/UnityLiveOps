using App.Runtime.Features.LiveOps.Models;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Services
{
    public interface ILiveOpExpirationHandler
    {
        bool IsExpired { get; }
        void UnloadIfExpired();
    }
}
