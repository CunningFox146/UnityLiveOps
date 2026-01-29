using System.Threading;
using App.Runtime.Features.LiveOps.Models;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.PlayGamesLiveOp.Services
{
    public interface IPlayGamesLiveOpUIHandler
    {
        void SetConfig(ILiveOpConfig config);
    }
}
