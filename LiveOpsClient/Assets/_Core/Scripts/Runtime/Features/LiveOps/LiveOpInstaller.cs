using App.Runtime.Features.ClickerLiveOp;
using App.Runtime.Features.LiveOps.Models;
using App.Shared.Mvc.Utils;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.LiveOps
{
    public abstract class LiveOpInstaller : IInstaller
    {
        private readonly LiveOpEvent _liveOpEvent;

        protected LiveOpInstaller(LiveOpEvent liveOpEvent)
        {
            _liveOpEvent = liveOpEvent;
        }
        
        public virtual void Install(IContainerBuilder builder)
        {
            builder.RegisterControllerServices();
            builder.RegisterController<EventIconController>();
            
            builder.RegisterInstance(_liveOpEvent);
        }
    }
}