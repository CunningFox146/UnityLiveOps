using System.Threading;
using App.Runtime.Features.LiveOps.Models;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.KeyCollectLiveOp.Services
{
    public interface IKeyCollectLiveOpUIHandler
    {
        void SetConfig(ILiveOpConfig config);
    }
}
