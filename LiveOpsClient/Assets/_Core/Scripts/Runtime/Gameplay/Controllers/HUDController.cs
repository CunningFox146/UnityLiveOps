using System.Threading;
using App.Runtime.Features.Common.Models;
using App.Runtime.Features.Common.Services;
using App.Runtime.Gameplay.Models;
using App.Runtime.Gameplay.Views;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.AssetManagement.Scope;
using App.Runtime.Services.Cameras;
using App.Shared.Mvc;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Gameplay.Controllers
{
    public class HUDController : ControllerBase
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IGameplayHandler _gameplayHandler;
        private readonly IFeatureService _featureService;
        private readonly ICameraProvider _cameraProvider;
        private IAssetScope _assetScope;
        private IHUDView _view;

        public HUDController(IAssetProvider assetProvider, IGameplayHandler gameplayHandler, IFeatureService featureService, ICameraProvider cameraProvider)
        {
            _assetProvider = assetProvider;
            _gameplayHandler = gameplayHandler;
            _featureService = featureService;
            _cameraProvider = cameraProvider;
        }

        protected override async UniTask OnStart(CancellationToken token)
        {
            _assetScope = new AssetScope(_assetProvider);
            _view = await _assetScope.InstantiateAsync<HUDView>(GameplayConstants.HUDViewPath, cancellationToken: token);
            _view.SetCamera(_cameraProvider.UICamera);
            _view.AddKeyClicked += OnAddKeyClicked;
            _view.ExitGameClicked += OnExitGameClicked;
            if (_featureService.IsFeatureActive(FeatureType.KeyCollectLiveOp))
                _view.ShowAddKeyButton();
        }

        protected override void OnStop()
        {
            _assetScope?.Dispose();
        }

        private void OnAddKeyClicked()
            => _gameplayHandler.Session.AddKey();

        private void OnExitGameClicked()
            => _gameplayHandler.RequestGameplayExit();
    }
}