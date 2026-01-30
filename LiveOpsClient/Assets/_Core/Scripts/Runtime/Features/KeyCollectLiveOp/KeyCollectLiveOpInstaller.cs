using App.Runtime.Features.KeyCollectLiveOp.Controllers;
using App.Runtime.Features.KeyCollectLiveOp.Model;
using App.Runtime.Features.KeyCollectLiveOp.Services;
using App.Runtime.Features.LiveOps;
using App.Runtime.Features.LiveOps.Models;
using App.Shared.Mvc.Utils;
using App.Shared.Repository;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.KeyCollectLiveOp
{
    public class KeyCollectLiveOpInstaller : LiveOpInstaller<KeyCollectLiveOpData>
    {
        public KeyCollectLiveOpInstaller(LiveOpState state) : base(state) { }

        public override void Install(IContainerBuilder builder)
        {
            base.Install(builder);
            
            builder.Register<IRepository<KeyCollectLiveOpData>, KeyCollectLiveOpRepository>(Lifetime.Scoped);
            builder.Register<KeyCollectLiveOpUIHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.RegisterController<KeyCollectLiveOpPopupController>();
            builder.RegisterEntryPoint<KeyCollectLiveOpEntryPoint>();
        }
    }
}
