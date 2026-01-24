using System;
using System.Threading;
using Core.Infrastructure.Logger;
using Core.Infrastructure.SceneLoader;
using Core.Infrastructure.Storage;
using CunningFox.LiveOps.Models;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace CunningFox
{
    public class BootEntryPoint : IAsyncStartable
    {
        private readonly ILogger _logger;
        private readonly ISceneLoaderService _sceneLoader;
        private readonly IPersistentStorage _storage;

        public BootEntryPoint(ILogger logger, ISceneLoaderService sceneLoader, IPersistentStorage storage)
        {
            _logger = logger;
            _sceneLoader = sceneLoader;
            _storage = storage;
        }
        
        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            try
            {
                await _storage.SaveAsync("poop", new LiveOpDto(Guid.NewGuid(), DateTime.Now, DateTime.Now, "test", 0), cancellation);
                var data = await _storage.LoadAsync<LiveOpDto>("poop", cancellation);
                
                _logger.Debug(data.ToString());
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