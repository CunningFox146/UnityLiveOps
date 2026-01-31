namespace App.Runtime.Features.Lobby.Controllers
{
    public readonly struct LobbyViewControllerArgs
    {
        public int PlayerLevel { get; }
        public LobbyViewControllerArgs(int playerLevel)
        {
            PlayerLevel = playerLevel;
        }
    }
}