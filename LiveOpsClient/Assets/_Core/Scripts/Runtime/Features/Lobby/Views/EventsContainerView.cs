using UnityEngine;

namespace App.Runtime.Features.Lobby.Views
{
    public class EventsContainerView : MonoBehaviour
    {
        [SerializeField] private RectTransform _leftGroup;
        [SerializeField] private RectTransform _rightGroup;
        [SerializeField] private int _maxEventsPerGroup;
        
        public RectTransform AvailableGroup => _leftGroup.childCount >= _maxEventsPerGroup ?  _rightGroup : _leftGroup;
    }
}