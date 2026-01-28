using App.Runtime.Features.Common.Views;
using App.Runtime.Services.AssetManagement.Scope;
using UnityEngine;

namespace App.Runtime.Features.Lobby.Models
{
    public readonly struct EventIconControllerArgs
    {
        public Transform IconParent { get; }
        public EventIconView IconPrefab { get; }
        
        public EventIconControllerArgs(Transform iconParent, EventIconView iconPrefab)
        {
            IconParent = iconParent;
            IconPrefab = iconPrefab;
        }
    }
}