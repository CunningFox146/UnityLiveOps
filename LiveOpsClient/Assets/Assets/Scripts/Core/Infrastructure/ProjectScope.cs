using Common;
using Common.Api;
using Common.Input;
using Common.Logger;
using Common.Monitoring;
using Common.Mvc.Factory;
using Common.Mvc.Service;
using Common.SceneLoader;
using Common.Storage;
using Core.Features.Lobby.Views;
using Core.Services.AssetProvider;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ILogger = Common.Logger.ILogger;

namespace Core.Infrastructure
{
    public class ProjectScope : LifetimeScope
    {
        private void OnEnable()
            => DontDestroyOnLoad(this);

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ILogger, UnityLogger>(Lifetime.Singleton);
            builder.Register<ISceneLoaderService, SceneLoaderService>(Lifetime.Singleton);
            builder.Register<IAssetProvider, ResourcesAssetProvider>(Lifetime.Singleton);
            
            #if UNITY_WEBGL
            builder.RegisterInstance<IHttpClient>(new UnityHttpClient("https://localhost:7158"));
            #else
            builder.RegisterInstance<IHttpClient>(new SystemHttpClient("https://localhost:7158"));
            #endif
            builder.Register<LiveOpsApiService>(Lifetime.Singleton);
            
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
