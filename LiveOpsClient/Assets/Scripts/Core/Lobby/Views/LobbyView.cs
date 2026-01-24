using System;
using System.Threading;
using Core.Core.Services.Views;
using CunningFox.AssetProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Lobby.Views
{
    public class LobbyView : MonoBehaviour, ILobbyView
    {
        public void Dispose()
        {
            Debug.Log("Disposed");
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
        
        protected override async UniTask StartFlow(CancellationToken token)
        {
            var prefab = await _assetProvider.LoadPrefab<LobbyView>("Views/BaseView", token);
            var view = Object.Instantiate(prefab);
            
            _view = view;
        }

        protected override void StopFlow()
        {
            _view.Dispose();
        }
    }
}