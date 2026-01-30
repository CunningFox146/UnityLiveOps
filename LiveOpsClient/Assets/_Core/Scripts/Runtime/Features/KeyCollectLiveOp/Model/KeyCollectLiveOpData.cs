using System;
using App.Runtime.Features.LiveOps.Models;

namespace App.Runtime.Features.KeyCollectLiveOp.Model
{
    public class KeyCollectLiveOpData : ILiveOpData
    {
        public int KeysCollected { get; set; }
        public DateTime EventStartTime { get; set; }
        
        public void Clear()
        {
            KeysCollected = 0;
            EventStartTime = DateTime.MinValue;
        }
    }
}
