using System;
using System.Collections.Generic;
using App.Runtime.Features.Common.Models;
using App.Shared.Time;
using CunningFox.LiveOps.Models;
using ZLinq;

namespace App.Runtime.Features.LiveOps.Models
{
    public class LiveOpsCalendar
    {
        public string Id { get; private set; }
        public List<LiveOpEvent> Events { get; private set; }
        public Dictionary<FeatureType, LiveOpState> SeenEvents { get; private set; }
        public TimeSpan TimeDifference { get; private set; }

        public static LiveOpsCalendar Empty => new()
        {
            Events = new List<LiveOpEvent>(),
            SeenEvents = new Dictionary<FeatureType, LiveOpState>(),
        };
        
        public void UpdateFromDto(LiveOpsCalendarDto dto, ITimeService timeService)
        {
            Id = dto.Id;
            Events = GetEventsFromDto(dto);
            TimeDifference = TimeSpan.FromTicks(timeService.Now.Ticks - dto.ServerTime);
            SeenEvents ??= new Dictionary<FeatureType, LiveOpState>();
        }

        public void RecordEvent(LiveOpState seenEvent)
        {
            if (SeenEvents.TryGetValue(seenEvent.Type, out var recorded)
                && recorded.EndTime > seenEvent.EndTime)
                return;

            SeenEvents[seenEvent.Type] = seenEvent;
        }

        private static List<LiveOpEvent> GetEventsFromDto(LiveOpsCalendarDto dto)
        {
            return dto.Events
                .AsValueEnumerable()
                .Select(LiveOpEvent.FromDto)
                .ToList();
        }
    }
}