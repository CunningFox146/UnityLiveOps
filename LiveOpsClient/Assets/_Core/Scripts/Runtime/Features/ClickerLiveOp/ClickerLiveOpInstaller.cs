using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.LiveOps;
using App.Shared.Repository;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.ClickerLiveOp
{
    public class ClickerLiveOpInstaller : LiveOpInstaller
    {
        public ClickerLiveOpInstaller(LiveOpState state) : base(state)
        {
        }

        public override void Install(IContainerBuilder builder)
        {
            base.Install(builder);
            
            builder.Register<IRepository<ClickerLiveOpData>, ClickerLiveOpRepository>(Lifetime.Scoped);
            builder.RegisterEntryPoint<ClickerLiveOpEntryPoint>();
        }
    }
}