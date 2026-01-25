using System;
using System.Threading;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.SceneLoader;
using App.Shared.Logger;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace App.Runtime.Infrastructure
{
    public class BootEntryPoint : IAsyncStartable
    {
        private readonly ILogger _logger;
        private readonly ISceneLoaderService _sceneLoader;
        private readonly IAssetProvider _assetProvider;

        public BootEntryPoint(ILogger logger, ISceneLoaderService sceneLoader, IAssetProvider assetProvider)
        {
            _logger = logger;
            _sceneLoader = sceneLoader;
            _assetProvider = assetProvider;
        }
        
        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            try
            {
                await _assetProvider.InitializeAsync(cancellation);
                await _sceneLoader.LoadSceneAsync("Lobby", cancellationToken: cancellation);
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                _logger.Error("Failed to load lobby", e);
            }
        }
    }
}