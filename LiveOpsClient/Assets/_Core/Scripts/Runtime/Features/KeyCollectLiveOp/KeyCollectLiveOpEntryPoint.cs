using System.Threading;
using App.Runtime.Features.KeyCollectLiveOp.Services;
using App.Runtime.Features.LiveOps;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services;
using App.Runtime.Services.AssetManagement.Provider;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.KeyCollectLiveOp
{
    public class KeyCollectLiveOpEntryPoint : LiveOpEntryPointBase
    {
        private readonly IKeyCollectLiveOpService _keyCollectService;
        private readonly IKeyCollectLiveOpUIHandler _uiHandler;
        
        public KeyCollectLiveOpEntryPoint(
            IAssetProvider assetProvider,
            ILiveOpIconHandler iconHandler,
            LiveOpState state,
            IKeyCollectLiveOpService keyCollectService,
            IKeyCollectLiveOpUIHandler uiHandler)
            : base(assetProvider, iconHandler, state)
        {
            _keyCollectService = keyCollectService;
            _uiHandler = uiHandler;
        }

        public override async UniTask StartAsync(CancellationToken token = default)
        {
            await _keyCollectService.Initialize(token);
            await base.StartAsync(token);
            _uiHandler.SetConfig(Config);
        }
    }
}
