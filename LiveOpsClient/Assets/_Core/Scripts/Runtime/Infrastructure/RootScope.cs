using App.Runtime.Features.Common;
using App.Runtime.Features.LiveOps.Api;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services;
using App.Runtime.Features.Lobby.Controllers;
using App.Runtime.Features.UserState.Models;
using App.Runtime.Features.UserState.Services;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.Input;
using App.Runtime.Services.SceneLoader;
using App.Runtime.Services.ViewStack;
using App.Shared.Api;
using App.Shared.Logger;
using App.Shared.Monitoring;
using App.Shared.Mvc.Factories;
using App.Shared.Mvc.Services;
using App.Shared.Repository;
using App.Shared.Storage;
using App.Shared.Time;
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
            
            builder.Register<IRepository<LiveOpsCalendar>, LiveOpsRepository>(Lifetime.Singleton);
            builder.Register<IRepository<ActiveUserState>, UserStateRepository>(Lifetime.Singleton);
            builder.Register<IUserStateService, UserStateService>(Lifetime.Singleton);
            
            builder.Register<IAssetProvider, AddressableAssetProvider>(Lifetime.Singleton);
            
            builder.Register<IViewStack, ViewStack>(Lifetime.Singleton);
            builder.Register<IControllerFactory, ControllerFactory>(Lifetime.Singleton);
            builder.Register<IControllerService, ControllerService>(Lifetime.Singleton);
            builder.Register<LobbyController>(Lifetime.Transient);
            
            builder.Register<PersistentStorage>(Lifetime.Singleton)
                .WithParameter(Application.persistentDataPath)
                .AsImplementedInterfaces();
            
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UnhandledExceptionMonitoringService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TimeService>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<LiveOpsService>(Lifetime.Singleton);
            builder.Register<ILiveOpsApiService, LiveOpsApiService>(Lifetime.Singleton);
            
            builder.Register<IFeatureService, FeatureService>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<BootEntryPoint>();
        }
    }
}
