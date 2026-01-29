using System;
using System.Collections.Generic;
using App.Runtime.Features.Common;
using App.Runtime.Features.Common.Models;

namespace App.Runtime.Features.Lobby.Models
{
    public class EventIconsHandler : IEventIconsHandler
    {
        public event Action IconAdded;
        public event Action<FeatureType> IconRemoved;
        public IReadOnlyDictionary<FeatureType, EventIconRegistration> RegisteredIcons => _registeredIcons;
        
        private readonly Dictionary<FeatureType, EventIconRegistration> _registeredIcons = new();

        public void UnregisterIcon(FeatureType key)
        {
            _registeredIcons.Remove(key);
            IconRemoved?.Invoke(key);
        }
        
        public void RegisterIcon(EventIconRegistration iconInfo)
        {
            if (_registeredIcons.ContainsKey(iconInfo.Key))
                return;
            
            _registeredIcons[iconInfo.Key] = iconInfo;
            IconAdded?.Invoke();
        }
    }
}