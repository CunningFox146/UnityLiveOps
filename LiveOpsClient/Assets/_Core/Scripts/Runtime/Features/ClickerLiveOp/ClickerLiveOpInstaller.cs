using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.LiveOps;
using App.Runtime.Features.LiveOps.Models;
using App.Shared.Mvc.Utils;
using App.Shared.Repository;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.ClickerLiveOp
{
    public class ClickerLiveOpInstaller : LiveOpInstaller
    {
        public ClickerLiveOpInstaller(LiveOpEvent liveOpEvent) : base(liveOpEvent)
        {
        }

        public override void Install(IContainerBuilder builder)
        {
            base.Install(builder);
            
            builder.Register<IRepository<ClickerLiveOpState>, ClickerLiveOpRepository>(Lifetime.Scoped);
            builder.RegisterEntryPoint<ClickerLiveOpEntryPoint>();
        }
    }
}