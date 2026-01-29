using System.Threading;

namespace App.Runtime.Features.LiveOps.Services.Scheduler
{
    public interface ILiveOpsEventScheduler
    {
        void ScheduleAllEvents(CancellationToken token);
    }
}
