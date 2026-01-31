using App.Runtime.Features.LiveOps.Models;

namespace App.Runtime.Features.KeyCollectLiveOp.Services
{
    public interface IKeyCollectLiveOpUIHandler
    {
        void SetConfig(ILiveOpConfig config);
    }
}
