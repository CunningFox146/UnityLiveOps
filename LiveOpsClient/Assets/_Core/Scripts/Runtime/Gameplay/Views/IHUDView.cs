using System;
using UnityEngine;

namespace App.Runtime.Gameplay.Views
{
    public interface IHUDView
    {
        event Action AddKeyClicked;
        event Action ExitGameClicked;
        void ShowAddKeyButton();
        void SetCamera(Camera uiCamera);
    }
}