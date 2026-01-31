using App.Runtime.Gameplay.Controllers;
using App.Runtime.Services.Cameras;
using App.Shared.Mvc.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Gameplay
{
    public class GameplayScope : LifetimeScope
    {
        [SerializeField] private CamerasRegistration _cameras;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_cameras);
            builder.RegisterControllerServices();
            builder.RegisterController<HUDController>();
            
            builder.RegisterEntryPoint<GameplayEntryPoint>();
        }
    }
}
