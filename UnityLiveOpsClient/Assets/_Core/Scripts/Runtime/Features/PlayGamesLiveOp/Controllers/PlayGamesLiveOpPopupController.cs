using System;
using System.Threading;
using App.Runtime.Features.PlayGamesLiveOp.Model;
using App.Runtime.Features.PlayGamesLiveOp.Views;
using App.Runtime.Services.ViewsFactory;
using App.Shared.Mvc;
using App.Shared.Utils;
using App.Shared.Logger;
using App.Shared.Repository;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.PlayGamesLiveOp.Controllers
{
    public class PlayGamesLiveOpPopupController : ControllerWithResult<PlayGamesLiveOpPopup, Empty>
    {
        private readonly IRepository<PlayGamesLiveOpData> _repository;
        private readonly IViewFactory _viewFactory;
        private readonly ILogger _logger;
        private PlayGamesLiveOpPopup _view;

        public PlayGamesLiveOpPopupController(IRepository<PlayGamesLiveOpData> repository, IViewFactory viewFactory, ILogger logger)
        {
            _repository = repository;
            _viewFactory = viewFactory;
            _logger = logger;
        }

        protected override async UniTask<Empty> OnStart(PlayGamesLiveOpPopup prefab, CancellationToken token)
        {
            try
            {
                _view = _viewFactory.CreateView(prefab);
                _view.SetGamesPlayed(_repository.Value.GamesPlayed);
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
