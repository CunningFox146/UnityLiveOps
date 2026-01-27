using System;
using UnityEngine;

namespace App.Runtime.Features.Lobby.Views
{
    public interface ILobbyView : IDisposable
    {
        Transform IconContainer { get; }
        void SetLevel(int level);
    }
}