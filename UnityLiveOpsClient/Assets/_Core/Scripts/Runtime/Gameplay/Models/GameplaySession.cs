namespace App.Runtime.Gameplay.Models
{
    public class GameplaySession
    {
        public int KeysCollected { get; private set; }

        public void AddKey()
            => KeysCollected++;
    }
}