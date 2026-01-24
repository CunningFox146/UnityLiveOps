using BestFiends.Features.Album;
using Core.Core.Services.Views;
using Core.Infrastructure.Logger;
using Core.Infrastructure.SceneLoader;
using Core.Input;
using Core.Lobby.Views;
using CunningFox.AssetProvider;
using CunningFox.Monitoring;
using VContainer;
using VContainer.Unity;

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
            
            builder.Register<IViewStack, ViewStack>(Lifetime.Singleton);
            builder.Register<IViewControllerFactory, ViewControllerFactory>(Lifetime.Singleton);
            builder.Register<IViewService, ViewService>(Lifetime.Singleton);
            builder.Register<LobbyViewController>(Lifetime.Singleton);
            
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UnhandledExceptionMonitoringService>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.RegisterEntryPoint<BootEntryPoint>();
        }
    }
}
