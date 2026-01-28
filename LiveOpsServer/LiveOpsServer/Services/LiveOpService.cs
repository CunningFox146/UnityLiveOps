using CunningFox.LiveOps.Models;

namespace CunningFox.LiveOpsServer.Services;

public class LiveOpService : ILiveOpService
{
    private static readonly TimeSpan CacheInterval = TimeSpan.FromMinutes(10);
    
    private readonly Lock _lock = new();
    private LiveOpsCalendarDto? _cachedCalendar;
    private DateTime _lastGeneratedAt = DateTime.MinValue;

    public LiveOpsCalendarDto GetCalendar()
    {
        lock (_lock)
        {
            if (_cachedCalendar is not null && DateTime.UtcNow - _lastGeneratedAt < CacheInterval)
                return _cachedCalendar;
            
            _cachedCalendar = GenerateCalendar();
            _lastGeneratedAt = DateTime.UtcNow;

            return _cachedCalendar;
        }
    }

    private static LiveOpsCalendarDto GenerateCalendar()
    {
        var now = DateTime.UtcNow;
        var events = new List<LiveOpDto>
        {
            new(Guid.NewGuid().ToString(), "0 17 * * Tue", TimeSpan.FromMinutes(0.5), "ClickerLiveOp", 0),
            new(Guid.NewGuid().ToString(), "0 17 * * Tue", TimeSpan.FromMinutes(10), "ClickerLiveOp", 0),
            new(Guid.NewGuid().ToString(), "0 17 * * Tue", TimeSpan.FromMinutes(60), "ClickerLiveOp", 0),
            new(Guid.NewGuid().ToString(), "0 17 * * Tue", TimeSpan.FromMinutes(120), "ClickerLiveOp", 0),
        };

        return new LiveOpsCalendarDto(Guid.NewGuid().ToString(), now.Ticks, events);
    }
}
