using VContainer;
using VContainer.Unity;

namespace Core.Lobby
{
    public class LobbyScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LobbyEntryPoint>(Lifetime.Scoped);
        }
    }
}