using System;
using System.Threading;
using App.Runtime.Features.KeyCollectLiveOp.Services;
using App.Runtime.Features.KeyCollectLiveOp.Views;
using App.Runtime.Services.Camera;
using App.Runtime.Services.ViewStack;
using App.Shared.Mvc;
using App.Shared.Utils;
using App.Shared.Logger;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace App.Runtime.Features.KeyCollectLiveOp.Controllers
{
    public class KeyCollectLiveOpPopupController : ControllerWithResult<KeyCollectLiveOpPopup, Empty>
    {
        private readonly IViewStack _viewStack;
        private readonly IKeyCollectLiveOpService _liveOpService;
        private readonly ICameraProvider _cameraProvider;
        private readonly ILogger _logger;
        private KeyCollectLiveOpPopup _view;

        public KeyCollectLiveOpPopupController(IViewStack viewStack, IKeyCollectLiveOpService liveOpService, ILogger logger)
        {
            _viewStack = viewStack;
            _liveOpService = liveOpService;
            _logger = logger;
        }

        protected override async UniTask<Empty> OnStart(KeyCollectLiveOpPopup prefab, CancellationToken token)
        {
            try
            {
                _view = Object.Instantiate(prefab);
                // _view.SetCamera(_cameraProvider.Camera);
                _view.SetKeysCollected(_liveOpService.KeysCollected);
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
