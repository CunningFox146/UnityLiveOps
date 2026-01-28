using App.Shared.Utils;
using TMPro;
using UnityEngine;

namespace App.Runtime.Features.Lobby.Views
{
    public class LobbyView : MonoBehaviour, ILobbyView
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private EventsContainerView _eventsContainer;
        [SerializeField] private TextMeshProUGUI _levelText;
        
        public Transform IconContainer => _eventsContainer.AvailableGroup;
        
        public void SetLevel(int level)
        {
            _levelText.text = $"Level {level}";
        }

        public void SetCamera(Camera uiCamera)
            => _canvas.worldCamera = uiCamera;

        public void Dispose()
            => this.DestroyBehaviourObject();
    }
}