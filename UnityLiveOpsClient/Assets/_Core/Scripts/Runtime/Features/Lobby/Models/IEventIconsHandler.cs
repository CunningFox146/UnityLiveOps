using System;
using System.Collections.Generic;
using App.Runtime.Features.Common.Models;

namespace App.Runtime.Features.Lobby.Models
{
    public interface IEventIconsHandler
    {
        event Action IconAdded;
        event Action<FeatureType> IconRemoved;
        IReadOnlyDictionary<FeatureType, EventIconRegistration> RegisteredIcons { get; }
        void RegisterIcon(EventIconRegistration iconInfo);
        void UnregisterIcon(FeatureType key);
    }
}