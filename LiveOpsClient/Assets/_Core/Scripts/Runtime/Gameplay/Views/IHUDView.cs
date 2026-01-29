using System;

namespace App.Runtime.Gameplay.Views
{
    public interface IHUDView
    {
        event Action AddKeyClicked;
        event Action ExitGameClicked;
        void ShowAddKeyButton();
    }
}