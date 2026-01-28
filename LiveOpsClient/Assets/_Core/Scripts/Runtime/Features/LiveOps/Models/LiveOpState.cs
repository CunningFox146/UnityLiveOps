using System;
using App.Runtime.Features.Common;
using App.Shared.Time;

namespace App.Runtime.Features.ClickerLiveOp.Model
{
    public class LiveOpState
    {
        public FeatureType FeatureType { get; set; } 
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeSpan ExpiresIn(ITimeService timeService)
            => EndTime - timeService.Now;
        public bool IsExpired(ITimeService timeService)
            => timeService.Now > EndTime;
    }
}