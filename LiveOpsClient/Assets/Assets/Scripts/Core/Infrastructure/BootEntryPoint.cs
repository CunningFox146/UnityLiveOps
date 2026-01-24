using System;
using System.Threading;
using Common.Assets.Scripts.Common.Logger;
using Common.Assets.Scripts.Common.SceneLoader;
using Core.Infrastructure.Logger;
using Core.Infrastructure.SceneLoader;
using CunningFox.LiveOps.Models;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace CunningFox
{
    public class BootEntryPoint : IAsyncStartable
    {
        private readonly ILogger _logger;
        private readonly ISceneLoaderService _sceneLoader;

        public BootEntryPoint(ILogger logger, ISceneLoaderService sceneLoader)
        {
            _logger = logger;
            _sceneLoader = sceneLoader;
        }
        
        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            try
            {
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