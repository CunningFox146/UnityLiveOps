using System.Threading;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.ClickerLiveOp.Services;
using App.Runtime.Features.LiveOps;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services;
using App.Runtime.Services.AssetManagement.Provider;
using App.Shared.Repository;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.ClickerLiveOp
{
    public class ClickerLiveOpEntryPoint : LiveOpEntryPointBase
    {
        private readonly IRepository<ClickerLiveOpData> _repository;
        private readonly ILiveOpDataLifecycle _dataLifecycle;
        private readonly LiveOpState _state;
        private readonly IClickerLiveOpUIHandler _uiHandler;
        
        public ClickerLiveOpEntryPoint(
            IAssetProvider assetProvider,
            ILiveOpIconHandler iconHandler,
            LiveOpState state,
            IRepository<ClickerLiveOpData> repository,
            ILiveOpDataLifecycle dataLifecycle,
            IClickerLiveOpUIHandler uiHandler)
            : base(assetProvider, iconHandler, state)
        {
            _repository = repository;
            _dataLifecycle = dataLifecycle;
            _state = state;
            _uiHandler = uiHandler;
        }

        public override async UniTask StartAsync(CancellationToken token = default)
        {
            await _dataLifecycle.RestoreAndValidateData(_repository, _state, token);
            await base.StartAsync(token);
            _uiHandler.SetConfig(Config);
        }
    }
}