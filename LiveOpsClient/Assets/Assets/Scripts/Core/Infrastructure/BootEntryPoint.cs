using System;
using System.Threading;
using Common.Logger;
using Common.SceneLoader;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Core.Infrastructure
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