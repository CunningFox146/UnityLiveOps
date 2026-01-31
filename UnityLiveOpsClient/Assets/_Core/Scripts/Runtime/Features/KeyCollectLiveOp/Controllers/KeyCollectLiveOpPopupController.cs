using System;
using System.Threading;
using App.Runtime.Features.KeyCollectLiveOp.Model;
using App.Runtime.Features.KeyCollectLiveOp.Views;
using App.Runtime.Services.ViewsFactory;
using App.Shared.Mvc;
using App.Shared.Utils;
using App.Shared.Logger;
using App.Shared.Repository;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.KeyCollectLiveOp.Controllers
{
    public class KeyCollectLiveOpPopupController : ControllerWithResult<KeyCollectLiveOpPopup, Empty>
    {
        private readonly IRepository<KeyCollectLiveOpData> _repository;
        private readonly IViewFactory _viewFactory;
        private readonly ILogger _logger;
        private KeyCollectLiveOpPopup _view;

        public KeyCollectLiveOpPopupController(IRepository<KeyCollectLiveOpData> repository, IViewFactory viewFactory, ILogger logger)
        {
            _repository = repository;
            _viewFactory = viewFactory;
            _logger = logger;
        }

        protected override async UniTask<Empty> OnStart(KeyCollectLiveOpPopup prefab, CancellationToken token)
        {
            try
            {
                _view = _viewFactory.CreateView(prefab);
                _view.SetKeysCollected(_repository.Value.KeysCollected);
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
