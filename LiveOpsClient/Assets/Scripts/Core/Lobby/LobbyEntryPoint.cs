using System;
using System.Threading;
using Core.Core.Services.Views;
using Core.Infrastructure.SceneLoader;
using Core.Lobby.Views;
using CunningFox.AssetProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Core.Lobby
{
    public class LobbyEntryPoint : IAsyncStartable, IDisposable
    {
        private readonly IViewService _viewService;
        private readonly ISceneLoaderService _sceneLoaderService;

        public LobbyEntryPoint(IViewService viewService, ISceneLoaderService sceneLoaderService)
        {
            _viewService = viewService;
            _sceneLoaderService = sceneLoaderService;
        }


        public async UniTask StartAsync(CancellationToken token = default)
        {
            _viewService.ShowView<LobbyViewController>(token).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: token);
            _sceneLoaderService.LoadSceneAsync("Test", cancellationToken: token).Forget();
        }
        
        public void Dispose()
        {
            
        }
    }
}