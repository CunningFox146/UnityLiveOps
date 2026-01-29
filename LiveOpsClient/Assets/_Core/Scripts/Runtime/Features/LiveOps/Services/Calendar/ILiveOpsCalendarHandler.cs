using System.Threading;
using App.Runtime.Features.LiveOps.Models;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Services.Calendar
{
    public interface ILiveOpsCalendarHandler
    {
        LiveOpsCalendar Calendar { get; }
        UniTask LoadFromServer(CancellationToken token);
        void SaveCalendar();
    }
}
