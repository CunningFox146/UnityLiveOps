using System;
using App.Runtime.Features.Common;
using App.Runtime.Features.Common.Models;
using App.Shared.Serialization;
using App.Shared.Time;
using CunningFox.LiveOps.Models;
using NCrontab;
using Newtonsoft.Json;

namespace App.Runtime.Features.LiveOps.Models
{
    public class LiveOpEvent
    {
        public string Id { get; private set; }
        public TimeSpan Duration { get; private set; }
        public CrontabSchedule Schedule { get; private set; }
        public FeatureType Type { get; private set; }
        public int EntryLevel { get; private set; }

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