using System;
using TMPro;
using UnityEngine;

namespace App.Runtime.Gameplay.Views
{
    public class HUDView : MonoBehaviour, IHUDView
    {
        public event Action AddKeyClicked;
        public event Action ExitGameClicked;
        
        [SerializeField] private GameObject _addKeyButton;

        public void ShowAddKeyButton()
            => _addKeyButton.SetActive(true);
        
        public void AddKey()
            => AddKeyClicked?.Invoke();

        public void ExitGame()
            => ExitGameClicked?.Invoke();
    }
}