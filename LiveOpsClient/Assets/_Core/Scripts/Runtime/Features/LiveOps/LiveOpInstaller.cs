using App.Runtime.Features.ClickerLiveOp;
using App.Runtime.Features.ClickerLiveOp.Model;
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
        }
    }
}