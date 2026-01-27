using System;
using System.Collections.Generic;
using System.Threading;
using App.Runtime.Services.AssetManagement.Scope;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Runtime.Features.Lobby.Models
{
    public interface IEventIconsHandler
    {
        event Action IconAdded;
        event Action<string> IconRemoved;
        Queue<EventIconRegistration> IconsQueue { get; }
        void RegisterIcon(EventIconRegistration iconInfo);
    }

    public class EventIconsHandler : IEventIconsHandler
    {
        public event Action IconAdded;
        public event Action<string> IconRemoved;
        public Queue<EventIconRegistration> IconsQueue { get; } = new();

        public void UnregisterIcon(string key)
        {
            IconRemoved?.Invoke(key);
        }
        
        public void RegisterIcon(EventIconRegistration iconInfo)
        {
            IconsQueue.Enqueue(iconInfo);
            IconAdded?.Invoke();
        }
    }

    public readonly struct EventIconRegistration
    {
        public string Key { get; }
        public Action<Transform, CancellationToken> Factory { get; }
        
        public EventIconRegistration(string key, Action<Transform, CancellationToken> factory)
        {
            Key = key;
            Factory = factory;
        }
    }
    
    public readonly struct EventIconControllerArgs
    {
        public IAssetScope Scope { get; }
        public Transform IconParent { get; }
        
        public EventIconControllerArgs(Transform iconParent, IAssetScope scope)
        {
            IconParent = iconParent;
            Scope = scope;
        }
    }
}