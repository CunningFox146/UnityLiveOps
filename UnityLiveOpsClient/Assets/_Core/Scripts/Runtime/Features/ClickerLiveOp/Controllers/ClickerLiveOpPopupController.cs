using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.ClickerLiveOp.Views;
using App.Runtime.Services.Cameras;
using App.Runtime.Services.ViewsFactory;
using App.Runtime.Services.ViewStack;
using App.Shared.Mvc;
using App.Shared.Utils;
using App.Shared.Logger;
using App.Shared.Repository;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.ClickerLiveOp.Controllers
{
    public class ClickerLiveOpPopupController : ControllerWithResult<ClickerLiveOpPopup, Empty>
    {
        private readonly IViewStack _viewStack;
        private readonly IRepository<ClickerLiveOpData> _repository;
        private readonly IViewFactory _viewFactory;
        private readonly ICameraProvider _cameraProvider;
        private readonly ILogger _logger;
        private ClickerLiveOpPopup _view;

        public ClickerLiveOpPopupController(IRepository<ClickerLiveOpData> repository, IViewFactory viewFactory, ILogger logger)
        {
            _repository = repository;
            _viewFactory = viewFactory;
            _logger = logger;
        }

        protected override async UniTask<Empty> OnStart(ClickerLiveOpPopup prefab, CancellationToken token)
        {
            try
            {
                _view = _viewFactory.CreateView(prefab);
                _view.SetProgress(_repository.Value.Progress);
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