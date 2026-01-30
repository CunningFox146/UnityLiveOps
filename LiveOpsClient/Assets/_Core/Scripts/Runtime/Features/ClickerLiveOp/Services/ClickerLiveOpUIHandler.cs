using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp.Controllers;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.ClickerLiveOp.Views;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services;
using App.Shared.Logger;
using App.Shared.Mvc.Services;
using App.Shared.Repository;
using App.Shared.Utils;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace App.Runtime.Features.ClickerLiveOp.Services
{
    public class ClickerLiveOpUIHandler : IClickerLiveOpUIHandler, IInitializable, IDisposable
    {
        private readonly IControllerService _controllerService;
        private readonly IRepository<ClickerLiveOpData> _repository;
        private readonly ILiveOpExpirationHandler _expirationHandler;
        private readonly LiveOpState _state;
        private readonly ILiveOpIconHandler _iconHandler;
        private readonly ILogger _logger;
        private readonly CancellationTokenSource _cts = new();
        private ClickerLiveOpPopup _popupPrefab;
        private CancellationToken Token => _cts.Token;

        public ClickerLiveOpUIHandler(
            IControllerService controllerService,
            IRepository<ClickerLiveOpData> repository,
            ILiveOpExpirationHandler expirationHandler,
            LiveOpState state,
            ILiveOpIconHandler iconHandler,
            ILogger logger)
        {
            _controllerService = controllerService;
            _repository = repository;
            _expirationHandler = expirationHandler;
            _state = state;
            _iconHandler = iconHandler;
            _logger = logger;
        }
        
        public void Initialize()
        {
            _iconHandler.IconClicked += IconHandlerOnIconClicked;
        }

        public void Dispose()
        {
            _iconHandler.IconClicked -= IconHandlerOnIconClicked;
            _cts.Cancel();
            _cts.Dispose();
        }

        public void SetConfig(ILiveOpConfig config)
        {
            if (config.PopupPrefab is not ClickerLiveOpPopup prefab)
            {
                _logger.Error($"Wrong popup type {config.PopupPrefab}");
                return;
            }
            _popupPrefab = prefab;
        }
        
        private async UniTask HandleIconClickAsync(CancellationToken token)
        {
            try
            {
                if (!_expirationHandler.IsExpired(_state))
                {
                    var data = _repository.Value;
                    data.Progress++;
                    _repository.Update(data);
                }
                
                await _controllerService.StartControllerWithResult<ClickerLiveOpPopupController, ClickerLiveOpPopup, Empty>(_popupPrefab, token);
                await _expirationHandler.UnloadIfExpired(_state);
                
                if (_expirationHandler.IsExpired(_state))
                    _repository.Clear();
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Failed to handle icon click", exception, LoggerTag.LiveOps);
            }
        }
        
        private void IconHandlerOnIconClicked()
            => HandleIconClickAsync(Token).Forget(_logger.LogUniTask);
    }
}
