using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Services
{
    public class LiveOpsService : ILiveOpsService
    {
        private readonly ILiveOpsCalendarHandler _calendarHandler;
        private readonly ILiveOpsEventScheduler _eventScheduler;

        public LiveOpsService(
            ILiveOpsCalendarHandler calendarHandler,
            ILiveOpsEventScheduler eventScheduler)
        {
            _calendarHandler = calendarHandler;
            _eventScheduler = eventScheduler;
        }

        public async UniTask Initialize(CancellationToken token)
        {
            await _calendarHandler.LoadFromServer(token);
            _eventScheduler.ScheduleAllEvents(token);
            _calendarHandler.SaveCalendar();
        }
    }
}