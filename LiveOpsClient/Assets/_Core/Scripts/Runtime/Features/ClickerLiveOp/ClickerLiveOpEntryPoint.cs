using System;
using System.Threading;
using App.Runtime.Features.Common;
using App.Runtime.Features.Lobby.Models;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.AssetManagement.Scope;
using App.Shared.Mvc.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace App.Runtime.Features.ClickerLiveOp
{
    public class ClickerLiveOpEntryPoint : IStartable, IDisposable
    {
        private readonly IEventIconsHandler _iconsHandler;
        private readonly IAssetProvider _assetProvider;
        private readonly IControllerService _controllerService;
        private AssetScope _assetScope;

        public ClickerLiveOpEntryPoint(IEventIconsHandler iconsHandler, IAssetProvider assetProvider,
            IControllerService controllerService)
        {
            _iconsHandler = iconsHandler;
            _assetProvider = assetProvider;
            _controllerService = controllerService;
        }

        public void Start()
        {
            _assetScope = new AssetScope(_assetProvider);
            RegisterLobbyIcon();
        }

        public void Dispose()
        {
            _assetScope?.Dispose();
        }

        private void RegisterLobbyIcon()
        {
            var info = new EventIconRegistration(FeatureType.LiveOpClicker, CreateLobbyIcon);
            _iconsHandler.RegisterIcon(info);
        }

        private void CreateLobbyIcon(Transform parent, CancellationToken token)
        {
            var args = new EventIconControllerArgs(parent, _assetScope);
            _controllerService.StartController<EventIconController, EventIconControllerArgs>(args, token);
        }
    }
}