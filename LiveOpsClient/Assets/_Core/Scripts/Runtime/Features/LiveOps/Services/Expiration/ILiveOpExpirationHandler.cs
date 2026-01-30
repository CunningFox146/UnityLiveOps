namespace App.Runtime.Features.LiveOps.Services
{
    public interface ILiveOpExpirationHandler
    {
        bool IsExpired { get; }
        void UnloadIfExpired();
    }
}
