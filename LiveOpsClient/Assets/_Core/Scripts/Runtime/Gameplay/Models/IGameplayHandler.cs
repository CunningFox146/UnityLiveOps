using System;
using App.Runtime.Gameplay.Models;

namespace App.Runtime.Gameplay.Services
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