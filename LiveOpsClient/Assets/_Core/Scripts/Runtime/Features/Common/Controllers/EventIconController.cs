using System;
using System.Threading;
using App.Runtime.Features.Common.Views;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.Lobby.Models;
using App.Shared.Mvc;
using App.Shared.Mvc.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ILogger = App.Shared.Logger.ILogger;
using Object = UnityEngine.Object;

namespace App.Runtime.Features.ClickerLiveOp
{
    public class EventIconController : ControllerBase<EventIconControllerArgs>
    {
        private readonly IControllerService _controllerService;
        private readonly ILogger _logger;
        private IEventIconView _view;
        private CancellationToken _token;

        public EventIconController(IControllerService controllerService, ILogger logger)
        {
            _controllerService = controllerService;
            _logger = logger;
        }

        protected override async UniTask OnStart(EventIconControllerArgs args, CancellationToken token)
        {
            try
            {
                _token = token;
                var assetScope = args.Scope;
                var config = await assetScope.LoadAssetAsync<ILiveOpConfig>("ClickerLiveOp/Config", token);
                _view = Object.Instantiate(config.IconPrefab, args.IconParent);
                _view.Clicked += OnViewClicked;
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Icon flow failed", exception);
            }
        }

        private void OnViewClicked()
        {
            Debug.Log("OnViewClicked");
            //_controllerService.StartController<EventPopupController>(_token);
        }

        protected override void OnStop()
        {
            _view.Clicked -= OnViewClicked;
            _view?.Dispose();   
        }
    }
}