using System;
using System.Threading;
using App.Runtime.Features.Lobby.Controllers;
using App.Runtime.Features.UserState.Service;
using App.Shared.Mvc.Service;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace App.Runtime.Features.Lobby
{
    public class LobbyEntryPoint : IAsyncStartable, IDisposable
    {
        private readonly IViewService _viewService;
        private readonly IUserStateService _userStateService;

        public LobbyEntryPoint(IViewService viewService, IUserStateService userStateService)
        {
            _viewService = viewService;
            _userStateService = userStateService;
        }
        
        public async UniTask StartAsync(CancellationToken token = default)
        {
            var args = new LobbyViewControllerArgs(_userStateService.CurrentLevel);
            await _viewService.ShowView<LobbyViewController, LobbyViewControllerArgs>(args, token);
        }
        
        public void Dispose()
        {
            
        }
    }
}