using TMPro;
using UnityEngine;

namespace App.Runtime.Features.Lobby.Views
{
    public class LobbyView : MonoBehaviour, ILobbyView
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        
        public void SetLevel(int level)
        {
            _levelText.text = $"Level {level}";
        }
        
        public void Dispose()
        {
            if (this != null)
                Destroy(gameObject);
        }
    }
}