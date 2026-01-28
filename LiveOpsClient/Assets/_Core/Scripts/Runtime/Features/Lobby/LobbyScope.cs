using App.Runtime.Features.Lobby.Controllers;
using App.Runtime.Services.Camera;
using App.Shared.Mvc.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.Lobby
{
    public class LobbyScope : LifetimeScope
    {
        [SerializeField] private Camera _uiCamera;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterControllerServices();
            builder.RegisterController<LobbyController>();
            builder.Register<ICameraProvider, CameraProvider>(Lifetime.Scoped)
                .WithParameter(_uiCamera);
            builder.RegisterEntryPoint<LobbyEntryPoint>(Lifetime.Scoped);
        }
    }
}