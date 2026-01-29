using System.Threading;
using App.Runtime.Features.LiveOps.Models;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.ClickerLiveOp.Services
{
    public interface IClickerLiveOpUIHandler
    {
        void SetConfig(ILiveOpConfig config);
    }
}
