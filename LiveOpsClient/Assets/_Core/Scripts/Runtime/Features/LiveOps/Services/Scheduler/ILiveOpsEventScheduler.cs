using System.Threading;

namespace App.Runtime.Features.LiveOps.Services
{
    public interface ILiveOpsEventScheduler
    {
        void ScheduleAllEvents(CancellationToken token);
    }
}
