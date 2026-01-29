using System;
using System.Threading;
using App.Runtime.Features.LiveOps.Models;
using App.Runtime.Features.LiveOps.Services;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.AssetManagement.Scope;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace App.Runtime.Features.LiveOps
{
    public abstract class LiveOpEntryPointBase : IAsyncStartable, IDisposable
    {
        protected readonly LiveOpState State;

        private readonly IAssetProvider _assetProvider;
        private readonly ILiveOpIconHandler _iconHandler;
        private AssetScope _assetScope;

        protected ILiveOpConfig Config { get; private set; }

        protected LiveOpEntryPointBase(
            IAssetProvider assetProvider,
            ILiveOpIconHandler iconHandler,
            LiveOpState state)
        {
            _assetProvider = assetProvider;
            _iconHandler = iconHandler;
            State = state;
        }

        public virtual UniTask StartAsync(CancellationToken token = default)
            => RegisterLiveOpIcon(token);

        public virtual void Dispose()
            => _assetScope?.Dispose();

        private async UniTask RegisterLiveOpIcon(CancellationToken token)
        {
            _assetScope = new AssetScope(_assetProvider);
            var path = ZString.Format(LiveOpConstants.LiveOpConfigName, State.Type);
            Config = await _assetScope.LoadAssetAsync<ILiveOpConfig>(path, token);
            _iconHandler.RegisterIcon(State, Config);
        }
    }
}
