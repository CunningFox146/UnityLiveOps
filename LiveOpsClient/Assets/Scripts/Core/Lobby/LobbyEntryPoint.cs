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

        public LobbyEntryPoint(IViewService viewService, LiveOpsApiService api)
        {
            _viewService = viewService;
            _api = api;
        }
        
        public async UniTask StartAsync(CancellationToken token = default)
        {
            _viewService.ShowView<LobbyViewController>(token).Forget();
            var result = await _api.GetCalendar(token);
            Debug.Log(result);
        }
        
        public void Dispose()
        {
            
        }
    }
}