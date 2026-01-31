using System;

namespace App.Runtime.Gameplay.Models
{
    public interface IGameplayHandler
    {
        event Action GameplayEnter;
        event Action<GameplaySession> GameplayExit;
        GameplaySession Session { get; }
        void RequestGameplayEnter();
        void RequestGameplayExit();
        void ClearSession();
    }
}