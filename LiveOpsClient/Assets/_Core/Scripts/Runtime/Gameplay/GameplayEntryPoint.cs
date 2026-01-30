using System.Threading;
using App.Runtime.Gameplay.Controllers;
using App.Runtime.Services.Cameras;
using App.Shared.Mvc.Services;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace App.Runtime.Gameplay
{
    public class GameplayEntryPoint : IAsyncStartable
    {
        private readonly IControllerService _controllerService;
        private readonly ICameraProvider _cameraProvider;
        private readonly CamerasRegistration _cameras;

        public GameplayEntryPoint(IControllerService controllerService, ICameraProvider cameraProvider, CamerasRegistration cameras)
        {
            _controllerService = controllerService;
            _cameraProvider = cameraProvider;
            _cameras = cameras;
        }

        public UniTask StartAsync(CancellationToken token = default)
        {
            _cameraProvider.SetCameras(_cameras);
            return _controllerService.StartController<HUDController>(token);
        }
    }
}
