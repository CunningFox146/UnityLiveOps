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
            if (_cachedCalendar is null || DateTime.UtcNow - _lastGeneratedAt >= CacheInterval)
            {
                _cachedCalendar = GenerateCalendar();
                _lastGeneratedAt = DateTime.UtcNow;
            }

            return _cachedCalendar;
        }
    }

    private static LiveOpsCalendarDto GenerateCalendar()
    {
        var now = DateTime.UtcNow;
        var events = new List<LiveOpDto>
        {
            new(Guid.NewGuid(), now.AddHours(-1), now.AddSeconds(10), "Test", 0),
            new(Guid.NewGuid(), now.AddHours(-5), now.AddMinutes(5), "Test", 3),
            new(Guid.NewGuid(), now.AddHours(-2), now.AddHours(1), "Test", 5),
            new(Guid.NewGuid(), now.AddHours(-2), now.AddMinutes(1), "Test", 7),
        };

        return new LiveOpsCalendarDto(Guid.NewGuid(), now.Ticks, events);
    }
}
