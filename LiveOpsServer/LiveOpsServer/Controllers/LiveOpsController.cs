using CunningFox.LiveOps.Models;
using CunningFox.LiveOpsServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace CunningFox.LiveOpsServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LiveOpsController(ILiveOpService liveOpService) : ControllerBase
{
    [HttpGet("ActiveId")]
    public string GetActiveLiveOpsId()
    {
        return liveOpService.GetCalendar().Id;
    }
    
    [HttpGet("Active")]
    public LiveOpsCalendarDto GetActiveLiveOps()
    {
        return liveOpService.GetCalendar();
    }
}
