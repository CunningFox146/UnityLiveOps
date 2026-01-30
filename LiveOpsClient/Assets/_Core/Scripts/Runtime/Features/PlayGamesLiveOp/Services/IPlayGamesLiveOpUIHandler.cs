using App.Runtime.Features.LiveOps.Models;

namespace App.Runtime.Features.PlayGamesLiveOp.Services
{
    public interface IPlayGamesLiveOpUIHandler
    {
        void SetConfig(ILiveOpConfig config);
    }
}
