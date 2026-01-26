using System.Threading;
using App.Runtime.Features.Lobby.Views;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.AssetManagement.Scope;
using App.Shared.Mvc;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.Lobby.Controllers
{
    public class LobbyViewController : ViewControllerBase<LobbyViewControllerArgs>
    {
        private readonly IAssetProvider _assetProvider;
        private ILobbyView _view;
        private IAssetScope _assetScope;

        public LobbyViewController(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        protected override async UniTask OnStart(LobbyViewControllerArgs args, CancellationToken token)
        {
            _assetScope = new AssetScope(_assetProvider);
            _view = await _assetScope.InstantiateAsync<LobbyView>("LobbyView", token);
            _view.SetLevel(args.PlayerLevel);
        }

        protected override void OnStop()
        {
            _assetScope?.Dispose();
            _view?.Dispose();
        }
    }
}