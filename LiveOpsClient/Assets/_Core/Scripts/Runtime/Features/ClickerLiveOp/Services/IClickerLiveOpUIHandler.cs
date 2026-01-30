using App.Runtime.Features.LiveOps.Models;

namespace App.Runtime.Features.ClickerLiveOp.Services
{
    public interface IClickerLiveOpUIHandler
    {
        void SetConfig(ILiveOpConfig config);
    }
}
