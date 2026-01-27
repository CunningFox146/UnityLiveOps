using System;
using System.Threading;
using App.Runtime.Features.Lobby.Controllers;
using App.Runtime.Features.UserState.Services;
using App.Shared.Mvc.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace App.Runtime.Features.Lobby
{
    public class LobbyEntryPoint : IAsyncStartable
    {
        private readonly IControllerService _controllerService;
        private readonly IUserStateService _userStateService;

        public LobbyEntryPoint(IControllerService controllerService, IUserStateService userStateService)
        {
            _controllerService = controllerService;
            _userStateService = userStateService;
        }
        
        public async UniTask StartAsync(CancellationToken token = default)
        {
            var args = new LobbyViewControllerArgs(_userStateService.CurrentLevel);
            await _controllerService.StartController<LobbyController, LobbyViewControllerArgs>(args, token);
        }
    }
}