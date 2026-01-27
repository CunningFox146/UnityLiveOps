using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace App.Runtime.Features.Lobby.Views
{
    public class LobbyView : MonoBehaviour, ILobbyView
    {
        [SerializeField] private EventsContainerView _eventsContainer;
        [SerializeField] private TextMeshProUGUI _levelText;
        
        public Transform IconContainer => _eventsContainer.AvailableGroup;
        
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