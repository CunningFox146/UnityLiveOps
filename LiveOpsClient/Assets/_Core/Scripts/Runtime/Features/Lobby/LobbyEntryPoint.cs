using System;
using System.Threading;
using App.Runtime.Features.Lobby.Controllers;
using App.Runtime.Services.SceneLoader;
using App.Shared.Mvc.Service;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace App.Runtime.Features.Lobby
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