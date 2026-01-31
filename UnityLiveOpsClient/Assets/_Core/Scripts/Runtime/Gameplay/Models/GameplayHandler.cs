using System;

namespace App.Runtime.Gameplay.Models
{
    public class GameplayHandler : IGameplayHandler
    {
        public event Action GameplayEnter;
        public event Action<GameplaySession> GameplayExit;
        public GameplaySession Session { get; private set; }

        public void RequestGameplayEnter()
            => GameplayEnter?.Invoke();

        public void RequestGameplayExit()
            => GameplayExit?.Invoke(Session);

        public void ClearSession()
            => Session = new GameplaySession();
    }
}