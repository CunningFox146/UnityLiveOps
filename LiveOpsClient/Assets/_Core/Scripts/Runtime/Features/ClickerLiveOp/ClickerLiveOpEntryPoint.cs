using System;
using System.Threading;
using App.Runtime.Features.Lobby.Models;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.AssetManagement.Scope;
using App.Shared.Mvc;
using App.Shared.Mvc.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using ILogger = App.Shared.Logger.ILogger;

namespace App.Runtime.Features.ClickerLiveOp
{
    public class ClickerLiveOpEntryPoint : IAsyncStartable, IDisposable
    {
        private readonly IEventIconsHandler _iconsHandler;
        private readonly IAssetProvider _assetProvider;
        private readonly IControllerService _controllerService;
        private readonly ILogger _logger;
        private AssetScope _assetScope;

        public ClickerLiveOpEntryPoint(IEventIconsHandler iconsHandler, IAssetProvider assetProvider, IControllerService controllerService, ILogger logger)
        {
            _iconsHandler = iconsHandler;
            _assetProvider = assetProvider;
            _controllerService = controllerService;
            _logger = logger;
        }
        
        public async UniTask StartAsync(CancellationToken token = default)
        {
            _assetScope = new AssetScope(_assetProvider);
            RegisterLobbyIcon(token);
        }
        
        private void RegisterLobbyIcon(CancellationToken token)
        {
            var info = new EventIconRegistration("Clicker", CreateLobbyIcon);
            _iconsHandler.RegisterIcon(info);
        }

        private void CreateLobbyIcon(Transform parent, CancellationToken token)
        {
            var args = new EventIconControllerArgs(parent,  _assetScope);
            _controllerService.StartController<EventIconController, EventIconControllerArgs>(args, token);
        }


        public void Dispose()
        {
            _assetScope?.Dispose();
        }
        
        
    }
    
    public class EventIconController : ControllerBase<EventIconControllerArgs>
    {
        private GameObject _icon;

        protected override async UniTask OnStart(EventIconControllerArgs args, CancellationToken token)
        {
            try
            {
                var assetScope = args.Scope;
                _icon = await assetScope.InstantiateAsync("EventIconBase", args.IconParent, token);
            }
            catch (Exception e)
            {
                
            }
        }

        protected override void OnStop()
        {
            if (_icon != null)
                GameObject.Destroy(_icon);
        }
    }
}