using App.Runtime.Features.Common;
using App.Runtime.Features.LiveOps.Api;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services;
using App.Runtime.Features.Lobby.Models;
using App.Runtime.Features.UserState.Models;
using App.Runtime.Features.UserState.Services;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.Input;
using App.Runtime.Services.SceneLoader;
using App.Runtime.Services.ViewStack;
using App.Shared.Api;
using App.Shared.Logger;
using App.Shared.Monitoring;
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
            RegisterCoreServices(builder);
            RegisterHttpClient(builder);
            RegisterUserState(builder);
            RegisterAssets(builder);
            RegisterLiveOps(builder);
            RegisterFeatureService(builder);

            builder.RegisterEntryPoint<BootEntryPoint>();
        }

        private static void RegisterCoreServices(IContainerBuilder builder)
        {
            builder.Register<ILogger, UnityLogger>(Lifetime.Singleton);
            builder.Register<ISceneLoaderService, SceneLoaderService>(Lifetime.Singleton);
            builder.Register<ViewStack>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UnhandledExceptionMonitoringService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TimeService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PersistentStorage>(Lifetime.Singleton)
                .WithParameter(Application.persistentDataPath)
                .AsImplementedInterfaces();
        }

        private static void RegisterFeatureService(IContainerBuilder builder)
        {
            builder.Register<IFeatureService, FeatureService>(Lifetime.Singleton);
            builder.Register<IEventIconsHandler, EventIconsHandler>(Lifetime.Singleton);
        }
        
        private static void RegisterLiveOps(IContainerBuilder builder)
        {
            builder.Register<ILiveOpsCalendarHandler, LiveOpsCalendarHandler>(Lifetime.Singleton);
            builder.Register<ILiveOpsEventScheduler, LiveOpsEventScheduler>(Lifetime.Singleton);
            builder.Register<ILiveOpsService, LiveOpsService>(Lifetime.Singleton);
            builder.Register<ILiveOpsApiService, LiveOpsApiService>(Lifetime.Singleton);
            builder.Register<IRepository<LiveOpsCalendar>, LiveOpsRepository>(Lifetime.Singleton);
        }

        private static void RegisterAssets(IContainerBuilder builder)
        {
            builder.Register<IAssetProvider, AddressableAssetProvider>(Lifetime.Singleton);
        }

        private static void RegisterUserState(IContainerBuilder builder)
        {
            builder.Register<IRepository<ActiveUserState>, UserStateRepository>(Lifetime.Singleton);
            builder.Register<IUserStateService, UserStateService>(Lifetime.Singleton);
        }

        private static void RegisterHttpClient(IContainerBuilder builder)
        {
#if UNITY_WEBGL
            builder.RegisterInstance<IHttpClient>(new UnityHttpClient("https://localhost:7158"));
#else
            builder.RegisterInstance<IHttpClient>(new SystemHttpClient("https://localhost:7158"));
#endif
        }
    }
}