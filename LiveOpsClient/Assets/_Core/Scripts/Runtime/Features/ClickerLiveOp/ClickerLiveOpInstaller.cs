using App.Shared.Mvc.Utils;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.ClickerLiveOp
{
    public class ClickerLiveOpInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterControllerServices();
            builder.RegisterController<EventIconController>();
            
            builder.RegisterEntryPoint<ClickerLiveOpEntryPoint>();
        }
    }
}