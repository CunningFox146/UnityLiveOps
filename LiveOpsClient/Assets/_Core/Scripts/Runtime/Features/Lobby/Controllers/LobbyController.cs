using System.Collections.Generic;
using System.Threading;
using App.Runtime.Features.Lobby.Models;
using App.Runtime.Features.Lobby.Views;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.AssetManagement.Scope;
using App.Shared.Mvc;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.Lobby.Controllers
{
    public class LobbyController : ControllerBase<LobbyViewControllerArgs>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IEventIconsHandler _iconsHandler;
        private readonly Dictionary<string, CancellationTokenSource> _activeIconsCts = new();
        private CancellationToken _token;
        private ILobbyView _view;
        private IAssetScope _assetScope;

        public LobbyController(IAssetProvider assetProvider, IEventIconsHandler iconsHandler)
        {
            _assetProvider = assetProvider;
            _iconsHandler = iconsHandler;
        }

        protected override async UniTask OnStart(LobbyViewControllerArgs args, CancellationToken token)
        {
            _token = token;
            _assetScope = new AssetScope(_assetProvider);
            _view = await _assetScope.InstantiateAsync<LobbyView>("LobbyView", cancellationToken: token);
            _view.SetLevel(args.PlayerLevel);

            _iconsHandler.IconAdded += HandleIconsQueue;
            _iconsHandler.IconRemoved += RemoveIcon;
            HandleIconsQueue();
        }

        private void RemoveIcon(string key)
        {
            if (!_activeIconsCts.Remove(key, out var cts))
                return;
            cts.Cancel();
            cts.Dispose();
        }

        protected override void OnStop()
        {
            _assetScope?.Dispose();
            _view?.Dispose();
        }
        
        private void HandleIconsQueue()
        {
            while (_iconsHandler.IconsQueue.TryDequeue(out var iconArgs))
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(_token);
                iconArgs.Factory(_view.IconContainer, cts.Token);
                _activeIconsCts[iconArgs.Key] = cts;
            }
        }
    }

    
}