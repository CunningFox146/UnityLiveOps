using System;
using App.Runtime.Features.LiveOps.Models;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.LiveOps.Factories
{
    public class LiveOpInstallerFactory : ILiveOpInstallerFactory
    {
        private readonly IObjectResolver _resolver;

        public LiveOpInstallerFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public IInstaller CreateInstaller(LiveOpState state)
            => _resolver.Resolve<Func<LiveOpState, IInstaller>>(state.Type)
                .Invoke(state);
    }
}