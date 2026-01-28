using App.Runtime.Services.AssetManagement.Scope;
using UnityEngine;

namespace App.Runtime.Features.Lobby.Models
{
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