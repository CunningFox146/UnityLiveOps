using CunningFox.LiveOps.Models;
using CunningFox.LiveOpsServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CunningFox.LiveOpsServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LiveOpsController(ILiveOpService liveOpService) : ControllerBase
{
    [HttpGet("active")]
    public LiveOpCalendarDto GetActiveLiveOps()
    {
        return liveOpService.GetCalendar();
    }
}
