using System;
using System.Threading;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.Common;
using App.Runtime.Features.LiveOps;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.Lobby.Models;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.AssetManagement.Scope;
using App.Shared.Logger;
using App.Shared.Mvc.Services;
using App.Shared.Time;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using ILogger = App.Shared.Logger.ILogger;

namespace App.Runtime.Features.ClickerLiveOp
{
    public class ClickerLiveOpEntryPoint : LiveOpEntryPointBase
    {
        public ClickerLiveOpEntryPoint(IEventIconsHandler iconsHandler, IAssetProvider assetProvider,
            IControllerService controllerService, ITimeService timeService, LiveOpState state, ILogger logger) : base(
            iconsHandler, assetProvider, controllerService, timeService, state, logger)
        {
        }
    }
}