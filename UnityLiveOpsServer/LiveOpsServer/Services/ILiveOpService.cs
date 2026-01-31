using CunningFox.LiveOps.Models;

namespace CunningFox.LiveOpsServer.Services;

public interface ILiveOpService
{
    LiveOpsCalendarDto GetCalendar();
}
