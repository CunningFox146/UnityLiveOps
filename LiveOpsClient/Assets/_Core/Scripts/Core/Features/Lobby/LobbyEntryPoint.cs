using System;
using System.Threading;
using Common.Api;
using Common.Mvc.Service;
using Common.SceneLoader;
using Core.Features.Lobby.Controllers;
using Core.Features.Lobby.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace Core.Features.Lobby
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