using System;
using System.Collections.Generic;
using App.Runtime.Features.Common;

namespace App.Runtime.Features.Lobby.Models
{
    public class EventIconsHandler : IEventIconsHandler
    {
        public event Action IconAdded;
        public event Action<FeatureType> IconRemoved;
        public Queue<EventIconRegistration> IconsQueue { get; } = new();

        public void UnregisterIcon(FeatureType key)
        {
            IconRemoved?.Invoke(key);
        }
        
        public void RegisterIcon(EventIconRegistration iconInfo)
        {
            IconsQueue.Enqueue(iconInfo);
            IconAdded?.Invoke();
        }
    }
}