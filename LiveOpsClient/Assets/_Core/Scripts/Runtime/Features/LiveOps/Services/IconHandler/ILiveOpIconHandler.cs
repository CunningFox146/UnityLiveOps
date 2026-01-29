using System;
using App.Runtime.Features.LiveOps.Models;

namespace App.Runtime.Features.LiveOps.Services
{
    public interface ILiveOpIconHandler
    {
        public event Action IconClicked;
        void RegisterIcon(LiveOpState state, ILiveOpConfig config);
    }
}