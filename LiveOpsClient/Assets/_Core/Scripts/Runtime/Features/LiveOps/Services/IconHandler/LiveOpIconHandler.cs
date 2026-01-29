using System;
using System.Threading;
using App.Runtime.Features.Common.Controllers;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.Lobby.Models;
using App.Shared.Logger;
using App.Shared.Mvc.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ILogger = App.Shared.Logger.ILogger;

namespace App.Runtime.Features.LiveOps.Services
{
    public class LiveOpIconHandler : ILiveOpIconHandler
    {
        public event Action IconClicked;
        
        private readonly IEventIconsHandler _iconsHandler;
        private readonly IControllerService _controllerService;
        private readonly ILogger _logger;
        private ILiveOpConfig _config;

        public LiveOpIconHandler(IEventIconsHandler iconsHandler, IControllerService controllerService, ILogger logger)
        {
            _iconsHandler = iconsHandler;
            _controllerService = controllerService;
            _logger = logger;
        }
        
        public void RegisterIcon(LiveOpState state, ILiveOpConfig config)
        {
            _config = config;
            var registration = new EventIconRegistration(state.Type, CreateIconAsync);
            _iconsHandler.RegisterIcon(registration);
        }

        private void CreateIconAsync(Transform parent, CancellationToken token)
            => CreateIconAsyncInternal(parent, _config, token).Forget();

        private async UniTaskVoid CreateIconAsyncInternal(Transform parent, ILiveOpConfig config, CancellationToken token)
        {
            try
            {
                var args = new EventIconControllerArgs(parent, config.IconPrefab, OnIconClicked);
                await _controllerService.StartController<EventIconController, EventIconControllerArgs>(args, token);
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Failed to create icon for LiveOp", exception, LoggerTag.LiveOps);
            }
        }

        private void OnIconClicked()
            => IconClicked?.Invoke();
    }
}
