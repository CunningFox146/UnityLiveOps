using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp.Services;
using App.Runtime.Features.ClickerLiveOp.Views;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Services.Camera;
using App.Runtime.Services.ViewStack;
using App.Shared.Mvc;
using App.Shared.Utils;
using App.Shared.Logger;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace App.Runtime.Features.ClickerLiveOp.Controllers
{
    public class ClickerLiveOpPopupController : ControllerWithResult<ClickerLiveOpPopup, Empty>
    {
        private readonly IViewStack _viewStack;
        private readonly IClickerLiveOpService _liveOpService;
        private readonly ICameraProvider _cameraProvider;
        private readonly LiveOpState _state;
        private readonly ILogger _logger;
        private ClickerLiveOpPopup _view;

        public ClickerLiveOpPopupController(IViewStack viewStack, IClickerLiveOpService liveOpService, LiveOpState state, ILogger logger)
        {
            _viewStack = viewStack;
            _liveOpService = liveOpService;
            _state = state;
            _logger = logger;
        }

        protected override async UniTask<Empty> OnStart(ClickerLiveOpPopup prefab, CancellationToken token)
        {
            try
            {
                _view = Object.Instantiate(prefab);
                // _view.SetCamera(_cameraProvider.Camera);
                _view.SetProgress(_liveOpService.Progress);
                _viewStack.Push(_view);
                await _view.WaitForCtaClick(token);
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Popup flow failed", exception);
            }

            return Empty.Default;
        }

        protected override void OnStop()
        {
            _view?.Dispose();   
        }
    }
}