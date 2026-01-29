using System.Collections.Generic;
using System.Threading;
using App.Runtime.Features.Common;
using App.Runtime.Features.Common.Models;
using App.Runtime.Features.Lobby.Behaviours;
using App.Runtime.Features.Lobby.Models;
using App.Runtime.Features.Lobby.Views;
using App.Runtime.Gameplay.Services;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.AssetManagement.Scope;
using App.Runtime.Services.Camera;
using App.Shared.Mvc;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.Lobby.Controllers
{
    public class LobbyController : ControllerBase<LobbyViewControllerArgs>
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IEventIconsHandler _iconsHandler;
        private readonly ICameraProvider _cameraProvider;
        private readonly IGameplayHandler _gameplayHandler;
        private readonly Dictionary<FeatureType, CancellationTokenSource> _activeIconsCts = new();
        private CancellationToken _token;
        private ILobbyView _view;
        private IParallaxController _parallaxController;
        private IAssetScope _assetScope;

        public LobbyController(IAssetProvider assetProvider, IEventIconsHandler iconsHandler, ICameraProvider cameraProvider, IGameplayHandler gameplayHandler)
        {
            _assetProvider = assetProvider;
            _iconsHandler = iconsHandler;
            _cameraProvider = cameraProvider;
            _gameplayHandler = gameplayHandler;
        }

        protected override async UniTask OnStart(LobbyViewControllerArgs args, CancellationToken token)
        {
            _token = token;
            _assetScope = new AssetScope(_assetProvider);
            _parallaxController = await _assetScope.InstantiateAsync<ParallaxView>(LobbyConstants.ParallaxViewPath, cancellationToken: token);
            _view = await _assetScope.InstantiateAsync<LobbyView>(LobbyConstants.LobbyViewPath, cancellationToken: token);
            _view.SetCamera(_cameraProvider.Camera);
            _view.SetLevel(args.PlayerLevel);
            _view.PlayButtonClicked += OnPlayButtonClicked;

            _iconsHandler.IconAdded += HandleIconsQueue;
            _iconsHandler.IconRemoved += RemoveIcon;
            HandleIconsQueue();
        }

        private void RemoveIcon(FeatureType key)
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
            _parallaxController?.Dispose();
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
        
        private void OnPlayButtonClicked()
            => _gameplayHandler.RequestGameplayEnter();
    }
}