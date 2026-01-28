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
    }
}