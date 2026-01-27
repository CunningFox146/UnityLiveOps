using System;
using System.Collections.Generic;
using System.Linq;
using App.Shared.Time;
using CunningFox.LiveOps.Models;
using NCrontab;
using ZLinq;

namespace App.Runtime.Features.LiveOps.Model
{
    public class LiveOpsCalendar
    {
        public Guid Id { get; private set; }
        public List<LiveOpEvent> Events { get; private set; }
        public List<Guid> SeenEvents { get; private set; }
        public TimeSpan TimeDifference { get; private set; }

        public static LiveOpsCalendar CreateFromDto(LiveOpsCalendarDto dto, ITimeService timeService)
            => new()
            {
                Id = dto.Id,
                Events = GetEventsFromDto(dto),
                SeenEvents = new List<Guid>(),
                TimeDifference = TimeSpan.FromTicks(timeService.Now.Ticks - dto.ServerTime)
            };

        private static List<LiveOpEvent> GetEventsFromDto(LiveOpsCalendarDto dto)
        {
            return dto.Events
                .AsValueEnumerable()
                .Select(LiveOpEvent.FromDto)
                .ToList();
        }
    }
}