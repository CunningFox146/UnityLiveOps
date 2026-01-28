using System;
using System.Threading;
using App.Runtime.Features.Lobby.Models;
using App.Shared.Mvc;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ILogger = App.Shared.Logger.ILogger;
using Object = UnityEngine.Object;

namespace App.Runtime.Features.ClickerLiveOp
{
    public class EventIconController : ControllerBase<EventIconControllerArgs>
    {
        private GameObject _icon;
        private readonly ILogger _logger;

        public EventIconController(ILogger logger)
        {
            _logger = logger;
        }

        protected override async UniTask OnStart(EventIconControllerArgs args, CancellationToken token)
        {
            try
            {
                var assetScope = args.Scope;
                _icon = await assetScope.InstantiateAsync("EventIconBase", args.IconParent, token);
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Icon flow failed", exception);
            }
        }

        protected override void OnStop()
        {
            if (_icon != null)
                Object.Destroy(_icon);
        }
    }
}