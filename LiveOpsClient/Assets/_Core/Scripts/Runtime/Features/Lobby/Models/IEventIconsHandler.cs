using System;
using System.Collections.Generic;
using App.Runtime.Features.Common;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.Lobby.Models
{
    public interface IEventIconsHandler
    {
        event Action IconAdded;
        event Action<FeatureType> IconRemoved;
        Queue<EventIconRegistration> IconsQueue { get; }
        void RegisterIcon(EventIconRegistration iconInfo);
    }
}