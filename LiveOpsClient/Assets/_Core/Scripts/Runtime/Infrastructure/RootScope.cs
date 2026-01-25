using App.Runtime.Features;
using App.Runtime.Features.Lobby.Controllers;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.Input;
using App.Runtime.Services.SceneLoader;
using App.Runtime.Services.ViewStack;
using App.Shared.Api;
using App.Shared.Logger;
using App.Shared.Monitoring;
using App.Shared.Mvc.Factory;
using App.Shared.Mvc.Service;
using App.Shared.Storage;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ILogger = App.Shared.Logger.ILogger;

namespace App.Runtime.Infrastructure
{
    public class RootScope : LifetimeScope
    {
        private void OnEnable()
        {
            name = $"DI {nameof(RootScope)}";
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ILogger, UnityLogger>(Lifetime.Singleton);
            builder.Register<ISceneLoaderService, SceneLoaderService>(Lifetime.Singleton);
            
            #if UNITY_WEBGL
            builder.RegisterInstance<IHttpClient>(new UnityHttpClient("https://localhost:7158"));
            #else
            builder.RegisterInstance<IHttpClient>(new SystemHttpClient("https://localhost:7158"));
            #endif
            
            builder.Register<IAssetProvider, AddressableAssetProvider>(Lifetime.Singleton);
            
            builder.Register<IViewStack, ViewStack>(Lifetime.Singleton);
            builder.Register<IViewControllerFactory, ViewControllerFactory>(Lifetime.Singleton);
            builder.Register<IViewService, ViewService>(Lifetime.Singleton);
            builder.Register<LobbyViewController>(Lifetime.Transient);
            
            builder.Register<PersistentStorage>(Lifetime.Singleton)
                .WithParameter(Application.persistentDataPath)
                .AsImplementedInterfaces();
            
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UnhandledExceptionMonitoringService>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.RegisterEntryPoint<BootEntryPoint>();
        }
    }
}
