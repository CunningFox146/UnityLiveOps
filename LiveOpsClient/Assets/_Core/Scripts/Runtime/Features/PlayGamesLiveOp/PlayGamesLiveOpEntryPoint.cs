using System.Threading;
using App.Runtime.Features.PlayGamesLiveOp.Model;
using App.Runtime.Features.PlayGamesLiveOp.Services;
using App.Runtime.Features.LiveOps;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services;
using App.Runtime.Gameplay.Models;
using App.Runtime.Gameplay.Services;
using App.Runtime.Services.AssetManagement.Provider;
using App.Shared.Repository;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.PlayGamesLiveOp
{
    public class PlayGamesLiveOpEntryPoint : LiveOpEntryPointBase
    {
        private readonly IRepository<PlayGamesLiveOpData> _repository;
        private readonly ILiveOpDataLifecycle _dataLifecycle;
        private readonly IGameplayHandler _gameplayHandler;
        private readonly LiveOpState _state;
        private readonly IPlayGamesLiveOpUIHandler _uiHandler;
        
        public PlayGamesLiveOpEntryPoint(
            IAssetProvider assetProvider,
            ILiveOpIconHandler iconHandler,
            LiveOpState state,
            IRepository<PlayGamesLiveOpData> repository,
            ILiveOpDataLifecycle dataLifecycle,
            IGameplayHandler gameplayHandler,
            IPlayGamesLiveOpUIHandler uiHandler)
            : base(assetProvider, iconHandler, state)
        {
            _repository = repository;
            _dataLifecycle = dataLifecycle;
            _gameplayHandler = gameplayHandler;
            _state = state;
            _uiHandler = uiHandler;
        }

        public override async UniTask StartAsync(CancellationToken token = default)
        {
            await _dataLifecycle.RestoreAndValidateData(_repository, _state, token);
            _gameplayHandler.GameplayExit += OnGameplayExit;
            await base.StartAsync(token);
            _uiHandler.SetConfig(Config);
        }

        private void OnGameplayExit(GameplaySession _)
        {
            var data = _repository.Value;
            data.GamesPlayed++;
            _repository.Update(data);
        }

        public override void Dispose()
        {
            _gameplayHandler.GameplayExit -= OnGameplayExit;
            base.Dispose();
        }
    }
}
