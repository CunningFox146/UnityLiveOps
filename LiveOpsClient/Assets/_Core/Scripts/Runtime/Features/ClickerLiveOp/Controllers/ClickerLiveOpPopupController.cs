using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.ClickerLiveOp.Views;
using App.Runtime.Features.Common.Views;
using App.Runtime.Services.ViewStack;
using App.Shared.Mvc;
using App.Shared.Time;
using App.Shared.Utils;
using App.Shared.Logger;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace App.Runtime.Features.ClickerLiveOp.Controllers
{
    public class ClickerLiveOpPopupController : ControllerWithResult<ClickerLiveOpPopup, Empty>
    {
        private readonly ITimeService _timeService;
        private readonly IViewStack _viewStack;
        private readonly LiveOpState _state;
        private readonly ILogger _logger;
        private ClickerLiveOpPopup _view;

        public ClickerLiveOpPopupController(ITimeService timeService, IViewStack viewStack, LiveOpState state, ILogger logger)
        {
            _timeService = timeService;
            _viewStack = viewStack;
            _state = state;
            _logger = logger;
        }

        protected override async UniTask<Empty> OnStart(ClickerLiveOpPopup prefab, CancellationToken token)
        {
            try
            {
                _view = Object.Instantiate(prefab);
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