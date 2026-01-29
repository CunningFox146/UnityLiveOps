using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.Common.Controllers;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.Lobby.Models;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.AssetManagement.Scope;
using App.Shared.Logger;
using App.Shared.Mvc.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using ILogger = App.Shared.Logger.ILogger;

namespace App.Runtime.Features.LiveOps
{
    public abstract class LiveOpEntryPointBase : IAsyncStartable, IDisposable
    {
        protected readonly IControllerService ControllerService;
        protected readonly ILogger Logger;
        protected readonly LiveOpState State;
        private readonly IEventIconsHandler _iconsHandler;
        private readonly IAssetProvider _assetProvider;

        private AssetScope _assetScope;
        protected ILiveOpConfig Config;

        protected LiveOpEntryPointBase(IEventIconsHandler iconsHandler, IAssetProvider assetProvider,
            IControllerService controllerService, LiveOpState liveOpState, ILogger logger)
        {
            _iconsHandler = iconsHandler;
            _assetProvider = assetProvider;
            ControllerService = controllerService;
            State = liveOpState;
            Logger = logger;
        }

        public virtual async UniTask StartAsync(CancellationToken token = default)
        {
            _assetScope = new AssetScope(_assetProvider);
            Config = await _assetScope.LoadAssetAsync<ILiveOpConfig>(State.Type + "/Config", token);
            RegisterLobbyIcon();
        }

        public virtual void Dispose()
            => _assetScope?.Dispose();

        protected abstract void OnIconClicked();
        
        private void RegisterLobbyIcon()
        {
            var info = new EventIconRegistration(State.Type, CreateLobbyIcon);
            _iconsHandler.RegisterIcon(info);
        }
        
        private void CreateLobbyIcon(Transform parent, CancellationToken token)
            => CreateLobbyIconAsync(parent, token).Forget();

        private async UniTaskVoid CreateLobbyIconAsync(Transform parent, CancellationToken token)
        {
            try
            {
                var args = new EventIconControllerArgs(parent, Config.IconPrefab, OnIconClicked);
                await ControllerService.StartController<EventIconController, EventIconControllerArgs>(args, token);
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                Logger.Error("Failed to show icon for liveOp", exception, LoggerTag.LiveOps);
            }
        }
    }
}