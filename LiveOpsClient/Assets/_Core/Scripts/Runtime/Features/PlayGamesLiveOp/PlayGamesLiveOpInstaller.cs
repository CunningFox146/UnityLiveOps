using App.Runtime.Features.PlayGamesLiveOp.Controllers;
using App.Runtime.Features.PlayGamesLiveOp.Model;
using App.Runtime.Features.PlayGamesLiveOp.Services;
using App.Runtime.Features.LiveOps;
using App.Runtime.Features.LiveOps.Models;
using App.Shared.Mvc.Utils;
using App.Shared.Repository;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.PlayGamesLiveOp
{
    public class PlayGamesLiveOpInstaller : LiveOpInstaller
    {
        public PlayGamesLiveOpInstaller(LiveOpState state) : base(state) { }

        public override void Install(IContainerBuilder builder)
        {
            base.Install(builder);
            
            builder.Register<IRepository<PlayGamesLiveOpData>, PlayGamesLiveOpRepository>(Lifetime.Scoped);
            builder.Register<IPlayGamesLiveOpService, PlayGamesLiveOpService>(Lifetime.Scoped);
            builder.Register<PlayGamesLiveOpUIHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.RegisterController<PlayGamesLiveOpPopupController>();
            builder.RegisterEntryPoint<PlayGamesLiveOpEntryPoint>();
        }
    }
}
