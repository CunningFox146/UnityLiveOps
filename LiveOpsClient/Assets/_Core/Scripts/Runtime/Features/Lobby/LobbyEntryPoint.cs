using System;
using System.Threading;
using App.Runtime.Features.Lobby.Controllers;
using App.Shared.Mvc.Service;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace App.Runtime.Features.Lobby
{
    public class LobbyEntryPoint : IAsyncStartable, IDisposable
    {
        private readonly IViewService _viewService;

        public LobbyEntryPoint(IViewService viewService)
        {
            _viewService = viewService;
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