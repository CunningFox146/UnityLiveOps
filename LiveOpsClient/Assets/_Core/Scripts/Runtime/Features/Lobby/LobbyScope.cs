using App.Runtime.Features.Lobby.Controllers;
using App.Runtime.Services.Cameras;
using App.Shared.Mvc.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.Lobby
{
    public class LobbyScope : LifetimeScope
    {
        [SerializeField] private CamerasRegistration _cameras;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_cameras);
            builder.RegisterControllerServices();
            builder.RegisterController<LobbyController>();
            
            builder.RegisterEntryPoint<LobbyEntryPoint>(Lifetime.Scoped);
        }
    }
}