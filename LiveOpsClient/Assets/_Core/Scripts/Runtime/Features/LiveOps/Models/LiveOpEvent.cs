using System;
using App.Runtime.Features.Common;
using App.Shared.Time;
using CunningFox.LiveOps.Models;
using NCrontab;

namespace App.Runtime.Features.LiveOps.Models
{
    public class LiveOpEvent
    {
        public Guid Id { get; set; }
        public TimeSpan Duration { get; set; }
        public CrontabSchedule Schedule { get; set; }
        public FeatureType Type { get; set; }
        public int EntryLevel { get; set; }

        public static LiveOpEvent FromDto(LiveOpDto dto)
            => new()
            {
                Id = dto.Id,
                Duration = dto.Duration,
                Type = dto.EventName.ToFeatureType(),
                EntryLevel = dto.EntryLevel,
                Schedule = CrontabSchedule.Parse(dto.Schedule),
            };

        public bool IsExpired(ITimeService timeService)
        {
            var now = timeService.Now;
            
            // Find the most recent occurrence that started before or at now
            var searchStart = now - Duration - TimeSpan.FromDays(365);
            
            DateTime? lastOccurrence = null;
            foreach (var occurrence in Schedule.GetNextOccurrences(searchStart, now))
            {
                lastOccurrence = occurrence;
            }
            
            // If no past occurrence found, the event hasn't started yet
            if (!lastOccurrence.HasValue)
                return true;
            
            // Event is expired if we're past its duration
            return now > lastOccurrence.Value + Duration;
        }
    }
}