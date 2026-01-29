using App.Runtime.Features.Common.Controllers;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services;
using App.Shared.Mvc.Utils;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.LiveOps
{
    public abstract class LiveOpInstaller : IInstaller
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
        }
    }
}