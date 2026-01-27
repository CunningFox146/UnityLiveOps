using App.Shared.Mvc.Factories;
using App.Shared.Mvc.Services;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.ClickerLiveOp
{
    public class ClickerLiveOpInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<IControllerFactory, ControllerFactory>(Lifetime.Scoped);
            builder.Register<IControllerService, ControllerService>(Lifetime.Scoped);
            
            builder.Register<EventIconController>(Lifetime.Transient);
            builder.RegisterEntryPoint<ClickerLiveOpEntryPoint>();
        }
    }
}