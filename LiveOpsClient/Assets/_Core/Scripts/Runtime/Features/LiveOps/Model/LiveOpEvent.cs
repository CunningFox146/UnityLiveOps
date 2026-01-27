using System;
using CunningFox.LiveOps.Models;
using NCrontab;

namespace App.Runtime.Features.LiveOps.Model
{
    public class LiveOpEvent
    {
        public Guid Id { get; set; }
        public TimeSpan Duration { get; set; }
        public CrontabSchedule Schedule { get; set; }
        public string BundleName { get; set; }
        public int EntryLevel { get; set; }

        public static LiveOpEvent FromDto(LiveOpDto dto)
            => new()
            {
                Id = dto.Id,
                Duration = dto.Duration,
                BundleName = dto.BundleName,
                EntryLevel = dto.EntryLevel,
                Schedule = CrontabSchedule.Parse(dto.Schedule),
            };
    }
}