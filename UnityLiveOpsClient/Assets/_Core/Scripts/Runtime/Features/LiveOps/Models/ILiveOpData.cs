using System;

namespace App.Runtime.Features.LiveOps.Models
{
    public interface ILiveOpData
    {
        DateTime EventStartTime { get; set; }
        void Clear();
    }
}
