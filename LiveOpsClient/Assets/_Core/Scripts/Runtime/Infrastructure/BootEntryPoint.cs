using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp;
using App.Runtime.Features.Common;
using App.Runtime.Features.Common.Services;
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
        private readonly ILiveOpsService _liveOpsService;

        public BootEntryPoint(ILogger logger, ISceneLoaderService sceneLoader, IAssetProvider assetProvider,
            IUserStateService userStateService, ILiveOpsService liveOpsService)
        {
            _logger = logger;
            _sceneLoader = sceneLoader;
            _assetProvider = assetProvider;
            _userStateService = userStateService;
            _liveOpsService = liveOpsService;
        }

        public async UniTask StartAsync(CancellationToken token = default)
        {
            try
            {
                await UniTask.WhenAll(
                    _assetProvider.InitializeAsync(token),
                    _userStateService.RestoreUserState(token)
                );
                await _liveOpsService.Initialize(token);
                await _sceneLoader.LoadSceneAsync("Lobby", cancellationToken: token);
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                _logger.Error("Failed to load lobby", e);
            }
        }
    }
}