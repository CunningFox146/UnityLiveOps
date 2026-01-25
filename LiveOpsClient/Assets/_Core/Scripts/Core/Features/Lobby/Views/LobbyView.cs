using UnityEngine;

namespace Core.Features.Lobby.Views
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