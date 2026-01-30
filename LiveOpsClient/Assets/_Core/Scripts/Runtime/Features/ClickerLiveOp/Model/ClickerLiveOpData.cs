using System;
using App.Runtime.Features.LiveOps.Models;

namespace App.Runtime.Features.ClickerLiveOp.Model
{
    public class ClickerLiveOpData : ILiveOpData
    {
        public int Progress { get; set; }
        public DateTime EventStartTime { get; set; }
        
        public void Clear()
        {
            Progress = 0;
            EventStartTime = DateTime.MinValue;
        }
    }
}