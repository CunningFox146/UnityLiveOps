using System;
using App.Runtime.Features.LiveOps.Models;

namespace App.Runtime.Features.PlayGamesLiveOp.Model
{
    public class PlayGamesLiveOpData : ILiveOpData
    {
        public int GamesPlayed { get; set; }
        public DateTime EventStartTime { get; set; }
        
        public void Clear()
        {
            GamesPlayed = 0;
            EventStartTime = DateTime.MinValue;
        }
    }
}
