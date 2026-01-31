using System.Threading;
using App.Runtime.Features.Lobby.Controllers;
using App.Runtime.Features.UserState.Services;
using App.Runtime.Services.Cameras;
using App.Shared.Mvc.Services;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace App.Runtime.Features.Lobby
{
    public class LobbyEntryPoint : IAsyncStartable
    {
        private readonly IControllerService _controllerService;
        private readonly IUserStateService _userStateService;
        private readonly ICameraProvider _cameraProvider;
        private readonly CamerasRegistration _cameras;

        public LobbyEntryPoint(IControllerService controllerService, IUserStateService userStateService,
            ICameraProvider cameraProvider, CamerasRegistration cameras)
        {
            _controllerService = controllerService;
            _userStateService = userStateService;
            _cameraProvider = cameraProvider;
            _cameras = cameras;
        }
        
        public UniTask StartAsync(CancellationToken token = default)
        {
            _cameraProvider.SetCameras(_cameras);
            var args = new LobbyViewControllerArgs(_userStateService.CurrentLevel);
            return _controllerService.StartController<LobbyController, LobbyViewControllerArgs>(args, token);
        }
    }
}