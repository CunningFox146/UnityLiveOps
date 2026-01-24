using System;
using System.Threading;
using Common.Mvc;
using Core.Services.AssetProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Features.Lobby.Views
{
    public class LobbyView : MonoBehaviour, ILobbyView
    {
        public void Dispose()
        {
            Debug.Log("Disposed");
            if (this != null)
                Destroy(gameObject);
        }
    }

    public interface ILobbyView : IDisposable
    {
    }

    public class LobbyViewController : ViewControllerBase
    {
        private readonly IAssetProvider _assetProvider;
        private ILobbyView _view;

        public LobbyViewController(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        protected override async UniTask OnStart(CancellationToken token)
        {
            var prefab = await _assetProvider.LoadPrefab<LobbyView>("Views/BaseView", token);
            _view = Object.Instantiate(prefab);
        }

        protected override void OnStop()
        {
            _view?.Dispose();
        }
    }
}