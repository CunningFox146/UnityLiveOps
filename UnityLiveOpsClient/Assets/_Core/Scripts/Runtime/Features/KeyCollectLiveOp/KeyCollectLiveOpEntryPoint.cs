using System.Threading;
using App.Runtime.Features.KeyCollectLiveOp.Model;
using App.Runtime.Features.KeyCollectLiveOp.Services;
using App.Runtime.Features.LiveOps;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services.IconHandler;
using App.Runtime.Features.LiveOps.Services.Lifecycle;
using App.Runtime.Gameplay.Models;
using App.Runtime.Services.AssetManagement.Provider;
using App.Shared.Repository;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.KeyCollectLiveOp
{
    public class KeyCollectLiveOpEntryPoint : LiveOpEntryPointBase
    {
        private readonly IRepository<KeyCollectLiveOpData> _repository;
        private readonly ILiveOpDataLifecycle _dataLifecycle;
        private readonly IGameplayHandler _gameplayHandler;
        private readonly LiveOpState _state;
        private readonly IKeyCollectLiveOpUIHandler _uiHandler;
        
        public KeyCollectLiveOpEntryPoint(
            IAssetProvider assetProvider,
            ILiveOpIconHandler iconHandler,
            LiveOpState state,
            IRepository<KeyCollectLiveOpData> repository,
            ILiveOpDataLifecycle dataLifecycle,
            IGameplayHandler gameplayHandler,
            IKeyCollectLiveOpUIHandler uiHandler)
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
            await _dataLifecycle.RestoreAndValidateData(token);
            _gameplayHandler.GameplayExit += OnGameplayExit;
            await base.StartAsync(token);
            _uiHandler.SetConfig(Config);
        }

        private void OnGameplayExit(GameplaySession session)
        {
            var data = _repository.Value;
            data.KeysCollected += session.KeysCollected;
            _repository.Update(data);
        }

        public override void Dispose()
        {
            _gameplayHandler.GameplayExit -= OnGameplayExit;
            base.Dispose();
        }
    }
}
