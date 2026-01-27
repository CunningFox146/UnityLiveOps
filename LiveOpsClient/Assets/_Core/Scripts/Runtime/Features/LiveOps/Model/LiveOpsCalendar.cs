using System;
using System.Collections.Generic;
using App.Shared.Time;
using CunningFox.LiveOps.Models;

namespace App.Runtime.Features.LiveOps.Model
{
    public class LiveOpsCalendar
    {
        public Guid Id { get; set; }
        public List<LiveOpDto> Events { get; set; }
        public List<Guid> SeenEvents { get; set; }
        public TimeSpan TimeDifference { get; set; }

        public static LiveOpsCalendar CreateFromDto(LiveOpsCalendarDto calendarDto, ITimeService timeService) =>
            new()
            {
                Id = calendarDto.Id,
                Events = calendarDto.Events,
                SeenEvents = new List<Guid>(),
                TimeDifference = TimeSpan.FromTicks(timeService.Now.Ticks - calendarDto.ServerTime)
            };
    }
}