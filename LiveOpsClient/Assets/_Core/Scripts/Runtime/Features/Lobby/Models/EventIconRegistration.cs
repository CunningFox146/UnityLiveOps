using System;
using System.Threading;
using App.Runtime.Features.Common;
using App.Runtime.Features.Common.Models;
using UnityEngine;

namespace App.Runtime.Features.Lobby.Models
{
    public readonly struct EventIconRegistration
    {
        public FeatureType Key { get; }
        public Action<Transform, CancellationToken> Factory { get; }
        
        public EventIconRegistration(FeatureType key, Action<Transform, CancellationToken> factory)
        {
            Key = key;
            Factory = factory;
        }
    }
}