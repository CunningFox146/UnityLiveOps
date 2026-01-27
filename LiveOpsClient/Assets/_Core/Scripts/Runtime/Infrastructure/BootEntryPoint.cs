using System;
using System.Threading;
using App.Runtime.Features.LiveOps.Services;
using App.Runtime.Features.UserState.Services;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.SceneLoader;
using Cysharp.Threading.Tasks;
using UnityEngine;
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
        private readonly LiveOpsService _liveOpsService;

        public BootEntryPoint(ILogger logger, ISceneLoaderService sceneLoader, IAssetProvider assetProvider,
            IUserStateService userStateService, LiveOpsService liveOpsService)
        {
            _logger = logger;
            _sceneLoader = sceneLoader;
            _assetProvider = assetProvider;
            _userStateService = userStateService;
            _liveOpsService = liveOpsService;
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