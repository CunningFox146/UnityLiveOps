using System;
using System.Threading;
using App.Runtime.Features.PlayGamesLiveOp.Services;
using App.Runtime.Features.PlayGamesLiveOp.Views;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Services.Camera;
using App.Runtime.Services.ViewStack;
using App.Shared.Mvc;
using App.Shared.Utils;
using App.Shared.Logger;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace App.Runtime.Features.PlayGamesLiveOp.Controllers
{
    public class PlayGamesLiveOpPopupController : ControllerWithResult<PlayGamesLiveOpPopup, Empty>
    {
        private readonly IViewStack _viewStack;
        private readonly IPlayGamesLiveOpService _liveOpService;
        private readonly ICameraProvider _cameraProvider;
        private readonly LiveOpState _state;
        private readonly ILogger _logger;
        private PlayGamesLiveOpPopup _view;

        public PlayGamesLiveOpPopupController(IViewStack viewStack, IPlayGamesLiveOpService liveOpService, LiveOpState state, ILogger logger)
        {
            _viewStack = viewStack;
            _liveOpService = liveOpService;
            _state = state;
            _logger = logger;
        }

        protected override async UniTask<Empty> OnStart(PlayGamesLiveOpPopup prefab, CancellationToken token)
        {
            try
            {
                _view = Object.Instantiate(prefab);
                // _view.SetCamera(_cameraProvider.Camera);
                _view.SetGamesPlayed(_liveOpService.GamesPlayed);
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
