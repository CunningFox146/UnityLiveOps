using System.Threading;
using App.Runtime.Features.ClickerLiveOp.Services;
using App.Runtime.Features.LiveOps;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services;
using App.Runtime.Services.AssetManagement.Provider;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.ClickerLiveOp
{
    public class ClickerLiveOpEntryPoint : LiveOpEntryPointBase
    {
        private readonly IClickerLiveOpService _clickerService;
        private readonly IClickerLiveOpUIHandler _uiHandler;
        
        public ClickerLiveOpEntryPoint(
            IAssetProvider assetProvider,
            ILiveOpIconHandler iconHandler,
            LiveOpState state,
            IClickerLiveOpService clickerService,
            IClickerLiveOpUIHandler uiHandler)
            : base(assetProvider, iconHandler, state)
        {
            _clickerService = clickerService;
            _uiHandler = uiHandler;
        }

        public override async UniTask StartAsync(CancellationToken token = default)
        {
            await _clickerService.Initialize(token);
            await base.StartAsync(token);
            _uiHandler.SetConfig(Config);
        }
    }
}