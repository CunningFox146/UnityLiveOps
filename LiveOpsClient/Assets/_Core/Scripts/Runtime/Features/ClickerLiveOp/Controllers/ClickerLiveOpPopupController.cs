using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.ClickerLiveOp.Views;
using App.Runtime.Services.Cameras;
using App.Runtime.Services.ViewStack;
using App.Shared.Mvc;
using App.Shared.Utils;
using App.Shared.Logger;
using App.Shared.Repository;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace App.Runtime.Features.ClickerLiveOp.Controllers
{
    public class ClickerLiveOpPopupController : ControllerWithResult<ClickerLiveOpPopup, Empty>
    {
        private readonly IViewStack _viewStack;
        private readonly IRepository<ClickerLiveOpData> _repository;
        private readonly ICameraProvider _cameraProvider;
        private readonly ILogger _logger;
        private ClickerLiveOpPopup _view;

        public ClickerLiveOpPopupController(IViewStack viewStack, IRepository<ClickerLiveOpData> repository,
            ICameraProvider cameraProvider, ILogger logger)
        {
            _viewStack = viewStack;
            _repository = repository;
            _cameraProvider = cameraProvider;
            _logger = logger;
        }

        protected override async UniTask<Empty> OnStart(ClickerLiveOpPopup prefab, CancellationToken token)
        {
            try
            {
                _view = Object.Instantiate(prefab);
                _view.SetCamera(_cameraProvider.UICamera);
                _view.SetProgress(_repository.Value.Progress);
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