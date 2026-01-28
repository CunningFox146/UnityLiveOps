using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp;
using App.Runtime.Features.Common;
using App.Runtime.Features.LiveOps.Services;
using App.Runtime.Features.UserState.Services;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.SceneLoader;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using ILogger = App.Shared.Logger.ILogger;

namespace App.Runtime.Infrastructure
{
    public class BootEntryPoint : IAsyncStartable
    {
        private readonly ILogger _logger;
        private readonly ISceneLoaderService _sceneLoader;
        private readonly IAssetProvider _assetProvider;
        private readonly IUserStateService _userStateService;
        private readonly ILiveOpsService _liveOpsService;
        private readonly IFeatureService _featureService;

        public BootEntryPoint(ILogger logger, ISceneLoaderService sceneLoader, IAssetProvider assetProvider,
            IUserStateService userStateService, ILiveOpsService liveOpsService, IFeatureService featureService)
        {
            _logger = logger;
            _sceneLoader = sceneLoader;
            _assetProvider = assetProvider;
            _userStateService = userStateService;
            _liveOpsService = liveOpsService;
            _featureService = featureService;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            try
            {
                await UniTask.WhenAll(
                    _assetProvider.InitializeAsync(cancellation),
                    _userStateService.RestoreUserState(cancellation)
                );
                await _liveOpsService.Initialize(cancellation);
                await _sceneLoader.LoadSceneAsync("Lobby", cancellationToken: cancellation);
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                _logger.Error("Failed to load lobby", e);
            }
        }
    }
}