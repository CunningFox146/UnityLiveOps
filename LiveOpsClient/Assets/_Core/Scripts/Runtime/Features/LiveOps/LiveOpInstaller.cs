using App.Runtime.Features.Common.Controllers;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services.Expiration;
using App.Runtime.Features.LiveOps.Services.IconHandler;
using App.Runtime.Features.LiveOps.Services.Lifecycle;
using App.Shared.Mvc.Utils;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.LiveOps
{
    public abstract class LiveOpInstaller<TData> : IInstaller
        where TData : ILiveOpData
    {
        private readonly LiveOpState _state;

        protected LiveOpInstaller(LiveOpState state)
        {
            _state = state;
        }

        public virtual void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(_state);
            builder.RegisterControllerServices();
            builder.RegisterController<EventIconController>();

            builder.Register<ILiveOpIconHandler, LiveOpIconHandler>(Lifetime.Scoped);
            builder.Register<ILiveOpDataLifecycle, LiveOpDataLifecycle>(Lifetime.Scoped);
            builder.Register<ILiveOpExpirationHandler, LiveOpExpirationHandler<TData>>(Lifetime.Scoped);
        }
    }
}