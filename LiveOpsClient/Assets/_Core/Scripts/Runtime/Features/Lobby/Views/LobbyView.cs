using UnityEngine;

namespace App.Runtime.Features.Lobby.Views
{
    public class LobbyView : MonoBehaviour, ILobbyView
    {
        public void Dispose()
        {
            if (this != null)
                Destroy(gameObject);
        }
    }
}