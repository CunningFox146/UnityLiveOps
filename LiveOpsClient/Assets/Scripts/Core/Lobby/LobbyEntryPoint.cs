using System;
using System.Threading;
using Core.Core.Services.Views;
using Core.Infrastructure.SceneLoader;
using Core.Lobby.Views;
using Core.Services.Api;
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
        private readonly LiveOpsApiService _api;
        private readonly ISceneLoaderService _sceneLoader;

        public LobbyEntryPoint(IViewService viewService, LiveOpsApiService api, ISceneLoaderService sceneLoader)
        {
            _viewService = viewService;
            _api = api;
            _sceneLoader = sceneLoader;
        }
        
        public async UniTask StartAsync(CancellationToken token = default)
        {
            await _viewService.ShowView<LobbyViewController>(token);
            Debug.Log("Shown");
        }
        
        public void Dispose()
        {
            
        }
    }
}