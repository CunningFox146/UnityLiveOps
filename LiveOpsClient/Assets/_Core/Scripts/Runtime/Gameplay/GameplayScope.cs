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
            builder.RegisterControllerServices();
            builder.RegisterController<HUDController>();
            builder.Register<ICameraProvider, CameraProvider>(Lifetime.Scoped)
                .WithParameter(_cameras);
            
            builder.RegisterEntryPoint<GameplayEntryPoint>();
        }
    }
}
