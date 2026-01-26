using System;

namespace App.Runtime.Features.Lobby.Views
{
    public interface ILobbyView : IDisposable
    {
        void SetLevel(int level);
    }
}