using System;
using System.Threading;
using App.Runtime.Features.Lobby.Views;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.AssetManagement.Scope;
using App.Shared.Mvc;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Runtime.Features.Lobby.Controllers
{
    public class LobbyViewController : ViewControllerBase
    {
        private readonly IAssetProvider _assetProvider;
        private ILobbyView _view;
        private IAssetScope _assetScope;

        public LobbyViewController(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        protected override async UniTask OnStart(CancellationToken token)
        {
            _assetScope = new AssetScope(_assetProvider);
            var prefab = await _assetScope.LoadAssetAsync<GameObject>("Views/BaseView.prefab", token);
            Object.Instantiate(prefab);
            
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
            _assetScope.Dispose();
        }

        protected override void OnStop()
        {
            _assetScope?.Dispose();
            _view?.Dispose();
        }
    }
}