using App.Runtime.Gameplay.Controllers;
using App.Runtime.Gameplay.Services;
using App.Shared.Mvc.Utils;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Gameplay
{
    public class GameplayScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterControllerServices();
            builder.RegisterController<HUDController>();
            builder.RegisterEntryPoint<GameplayEntryPoint>();
        }
    }
}
