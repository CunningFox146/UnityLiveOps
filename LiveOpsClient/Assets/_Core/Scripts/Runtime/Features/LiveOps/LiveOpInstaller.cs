using App.Runtime.Features.ClickerLiveOp;
using App.Shared.Mvc.Utils;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.LiveOps
{
    public class LiveOpInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterControllerServices();
            builder.RegisterController<EventIconController>();
        }
    }
}