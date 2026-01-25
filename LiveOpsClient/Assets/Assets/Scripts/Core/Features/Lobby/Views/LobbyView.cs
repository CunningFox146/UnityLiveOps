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
        private IAssetScope _assetScope;

        public LobbyViewController(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        protected override async UniTask OnStart(CancellationToken token)
        {
            _assetScope = new AssetScope(_assetProvider);
            var prefab = await _assetScope.LoadAssetAsync<GameObject>("Views/BaseView.prefab", token);
            var inst = Object.Instantiate(prefab);
            
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
            Object.Destroy(inst);
            _assetScope.Dispose();
            
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
            Object.Instantiate(prefab);
        }

        protected override void OnStop()
        {
            _assetScope?.Dispose();
            _view?.Dispose();
        }
    }
}