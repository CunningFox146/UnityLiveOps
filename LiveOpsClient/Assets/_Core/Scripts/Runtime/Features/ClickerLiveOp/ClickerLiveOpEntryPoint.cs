using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp.Controllers;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.ClickerLiveOp.Services;
using App.Runtime.Features.ClickerLiveOp.Views;
using App.Runtime.Features.Common;
using App.Runtime.Features.Common.Services;
using App.Runtime.Features.LiveOps;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.Lobby.Models;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.AssetManagement.Scope;
using App.Shared.Logger;
using App.Shared.Mvc.Services;
using App.Shared.Time;
using App.Shared.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using ILogger = App.Shared.Logger.ILogger;

namespace App.Runtime.Features.ClickerLiveOp
{
    public class ClickerLiveOpEntryPoint : LiveOpEntryPointBase
    {
        private readonly ITimeService _timeService;
        private readonly IFeatureService _featureService;
        private readonly IClickerLiveOpService _clickerService;
        private CancellationToken _token;

        public ClickerLiveOpEntryPoint(IEventIconsHandler iconsHandler, IAssetProvider assetProvider,
            IControllerService controllerService, LiveOpState liveOpState, ILogger logger, ITimeService timeService,
            IFeatureService featureService, IClickerLiveOpService clickerService)
            : base(iconsHandler, assetProvider, controllerService, liveOpState, logger)
        {
            _timeService = timeService;
            _featureService = featureService;
            _clickerService = clickerService;
        }

        public override async UniTask StartAsync(CancellationToken token = default)
        {
            _token = token;
            await _clickerService.Initialize(token);
            await base.StartAsync(token);
        }

        protected override void OnIconClicked()
        {
            _clickerService.IncrementProgress();
            ShowPopup(_token).Forget();
        }

        private async UniTask ShowPopup(CancellationToken token)
        {
            try
            {
                var popupPrefab = (ClickerLiveOpPopup)Config.PopupPrefab;
                await ControllerService
                    .StartControllerWithResult<ClickerLiveOpPopupController, ClickerLiveOpPopup, Empty>(popupPrefab,
                        token);

                if (State.IsExpired(_timeService))
                    _featureService.StopFeature(State.Type);
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                Logger.Error("Failed to show popup", exception, LoggerTag.LiveOps);
            }
        }
    }
}