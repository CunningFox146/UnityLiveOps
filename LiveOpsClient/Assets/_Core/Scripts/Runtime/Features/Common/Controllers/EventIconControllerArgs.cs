using System;
using App.Runtime.Features.Common.Views;
using UnityEngine;

namespace App.Runtime.Features.Lobby.Models
{
    public readonly struct EventIconControllerArgs
    {
        public Transform IconParent { get; }
        public EventIconView IconPrefab { get; }
        public Action IconClicked { get; } 
        
        public EventIconControllerArgs(Transform iconParent, EventIconView iconPrefab, Action iconClicked)
        {
            IconParent = iconParent;
            IconPrefab = iconPrefab;
            IconClicked = iconClicked;
        }
    }
}