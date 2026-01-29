using System.Threading;
using App.Runtime.Features.PlayGamesLiveOp.Services;
using App.Runtime.Features.LiveOps;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services;
using App.Runtime.Services.AssetManagement.Provider;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.PlayGamesLiveOp
{
    public class PlayGamesLiveOpEntryPoint : LiveOpEntryPointBase
    {
        private readonly IPlayGamesLiveOpService _playGamesService;
        private readonly IPlayGamesLiveOpUIHandler _uiHandler;
        
        public PlayGamesLiveOpEntryPoint(
            IAssetProvider assetProvider,
            ILiveOpIconHandler iconHandler,
            LiveOpState state,
            IPlayGamesLiveOpService playGamesService,
            IPlayGamesLiveOpUIHandler uiHandler)
            : base(assetProvider, iconHandler, state)
        {
            _playGamesService = playGamesService;
            _uiHandler = uiHandler;
        }

        public override async UniTask StartAsync(CancellationToken token = default)
        {
            await _playGamesService.Initialize(token);
            await base.StartAsync(token);
            _uiHandler.SetConfig(Config);
        }
    }
}
