namespace App.Runtime.Features.LiveOps.Services.Expiration
{
    public interface ILiveOpExpirationHandler
    {
        bool IsExpired { get; }
        void UnloadIfExpired();
    }
}
