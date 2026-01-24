using Common.Assets.Scripts.Common.Api;
using Common.Assets.Scripts.Common.Input;
using Common.Assets.Scripts.Common.Logger;
using Common.Assets.Scripts.Common.Monitoring;
using Common.Assets.Scripts.Common.SceneLoader;
using Common.Assets.Scripts.Common.Storage;
using Core.Core.Services.Views;
using Core.Infrastructure.Logger;
using Core.Infrastructure.SceneLoader;
using Core.Infrastructure.Storage;
using Core.Input;
using Core.Lobby.Views;
using Core.Services.Api;
using Core.Services.Views;
using CunningFox.AssetProvider;
using CunningFox.Monitoring;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ILogger = Common.Assets.Scripts.Common.Logger.ILogger;

namespace CunningFox
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
