using System;
using System.Threading;
using App.Runtime.Features.PlayGamesLiveOp.Model;
using App.Runtime.Features.PlayGamesLiveOp.Views;
using App.Runtime.Services.Camera;
using App.Runtime.Services.ViewStack;
using App.Shared.Mvc;
using App.Shared.Utils;
using App.Shared.Logger;
using App.Shared.Repository;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace App.Runtime.Features.PlayGamesLiveOp.Controllers
{
    public class PlayGamesLiveOpPopupController : ControllerWithResult<PlayGamesLiveOpPopup, Empty>
    {
        private readonly IViewStack _viewStack;
        private readonly IRepository<PlayGamesLiveOpData> _repository;
        private readonly ICameraProvider _cameraProvider;
        private readonly ILogger _logger;
        private PlayGamesLiveOpPopup _view;

        public PlayGamesLiveOpPopupController(IViewStack viewStack, IRepository<PlayGamesLiveOpData> repository, ILogger logger)
        {
            _viewStack = viewStack;
            _repository = repository;
            _logger = logger;
        }

        protected override async UniTask<Empty> OnStart(PlayGamesLiveOpPopup prefab, CancellationToken token)
        {
            try
            {
                _view = Object.Instantiate(prefab);
                // _view.SetCamera(_cameraProvider.Camera);
                _view.SetGamesPlayed(_repository.Value.GamesPlayed);
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
