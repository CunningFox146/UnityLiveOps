using System.Threading;
using App.Runtime.Gameplay.Controllers;
using App.Shared.Mvc.Services;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace App.Runtime.Gameplay
{
    public class GameplayEntryPoint : IAsyncStartable
    {
        private readonly IControllerService _controllerService;

        public GameplayEntryPoint(IControllerService controllerService)
        {
            _controllerService = controllerService;
        }

        public UniTask StartAsync(CancellationToken token = default)
        {
            return _controllerService.StartController<HUDController>(token);
        }
    }
}
