using System;
using System.Threading;
using App.Runtime.Features.KeyCollectLiveOp.Controllers;
using App.Runtime.Features.KeyCollectLiveOp.Views;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services.Expiration;
using App.Runtime.Features.LiveOps.Services.IconHandler;
using App.Shared.Logger;
using App.Shared.Mvc.Services;
using App.Shared.Utils;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace App.Runtime.Features.KeyCollectLiveOp.Services
{
    public class KeyCollectLiveOpUIHandler : IKeyCollectLiveOpUIHandler, IInitializable, IDisposable
    {
        private readonly IControllerService _controllerService;
        private readonly ILiveOpExpirationHandler _expirationHandler;
        private readonly ILiveOpIconHandler _iconHandler;
        private readonly ILogger _logger;
        private readonly CancellationTokenSource _cts = new();
        private KeyCollectLiveOpPopup _popupPrefab;
        
        private CancellationToken Token => _cts.Token;

        public KeyCollectLiveOpUIHandler(
            IControllerService controllerService,
            ILiveOpExpirationHandler expirationHandler,
            ILiveOpIconHandler iconHandler,
            ILogger logger)
        {
            _controllerService = controllerService;
            _expirationHandler = expirationHandler;
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
            _cts.CancelAndDispose();
        }

        public void SetConfig(ILiveOpConfig config)
        {
            if (config.PopupPrefab is not KeyCollectLiveOpPopup prefab)
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
                await _controllerService.StartControllerWithResult<KeyCollectLiveOpPopupController, KeyCollectLiveOpPopup, Empty>(_popupPrefab, token);
                _expirationHandler.UnloadIfExpired();
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
