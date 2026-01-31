using System;
using App.Runtime.Features.Common.Models;
using App.Shared.Time;

namespace App.Runtime.Features.LiveOps.Models
{
    public class LiveOpState : IEquatable<LiveOpState>
    {

        public FeatureType Type { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public LiveOpState(FeatureType type, DateTime startTime, DateTime endTime)
        {
            Type = type;
            StartTime = startTime;
            EndTime = endTime;
        }
        
        public TimeSpan ExpiresIn(ITimeService timeService)
            => EndTime - timeService.Now;
        public bool IsExpired(ITimeService timeService)
            => timeService.Now > EndTime;
        
        public override int GetHashCode()
            => HashCode.Combine((int)Type, StartTime, EndTime);
        
        public bool Equals(LiveOpState other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type && StartTime.Equals(other.StartTime) && EndTime.Equals(other.EndTime);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((LiveOpState)obj);
        }
    }
}