using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.Common;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.Lobby.Models;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.AssetManagement.Scope;
using App.Shared.Logger;
using App.Shared.Mvc.Services;
using App.Shared.Time;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using ILogger = App.Shared.Logger.ILogger;

namespace App.Runtime.Features.LiveOps
{
    public abstract class LiveOpEntryPointBase : IStartable, IDisposable
    {
        private readonly IEventIconsHandler _iconsHandler;
        private readonly IAssetProvider _assetProvider;
        private readonly IControllerService _controllerService;
        private readonly ITimeService _timeService;
        private readonly LiveOpState _state;
        private readonly ILogger _logger;
        private AssetScope _assetScope;

        protected LiveOpEntryPointBase(IEventIconsHandler iconsHandler, IAssetProvider assetProvider,
            IControllerService controllerService, ITimeService timeService, LiveOpState state, ILogger logger)
        {
            _iconsHandler = iconsHandler;
            _assetProvider = assetProvider;
            _controllerService = controllerService;
            _timeService = timeService;
            _state = state;
            _logger = logger;
        }

        public virtual void Start()
        {
            _assetScope = new AssetScope(_assetProvider);
            RegisterLobbyIcon();
        }

        public virtual void Dispose()
        {
            _assetScope?.Dispose();
        }

        private void RegisterLobbyIcon()
        {
            var info = new EventIconRegistration(_state.Type, CreateLobbyIcon);
            _iconsHandler.RegisterIcon(info);
        }

        private void CreateLobbyIcon(Transform parent, CancellationToken token)
        {
            CreateLobbyIconAsync(parent, token).Forget();
        }

        private async UniTaskVoid CreateLobbyIconAsync(Transform parent, CancellationToken token)
        {
            try
            {
                var settings = await _assetScope.LoadAssetAsync<ILiveOpConfig>(_state.Type + "/Config", token);
                var args = new EventIconControllerArgs(parent, settings.IconPrefab);
                await _controllerService.StartController<EventIconController, EventIconControllerArgs>(args, token);
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                _logger.Error("Failed to show icon for liveOp", exception, LoggerTag.LiveOps);
            }
        }
    }
}