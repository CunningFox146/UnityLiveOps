using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.Lobby
{
    public class LobbyScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LobbyEntryPoint>(Lifetime.Scoped);
        }
    }
}