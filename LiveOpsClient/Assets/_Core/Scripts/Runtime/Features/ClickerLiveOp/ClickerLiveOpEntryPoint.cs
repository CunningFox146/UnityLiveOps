using System;
using App.Runtime.Services.AssetManagement.Provider;
using App.Runtime.Services.AssetManagement.Scope;
using VContainer.Unity;

namespace App.Runtime.Features.ClickerLiveOp
{
    public class ClickerLiveOpEntryPoint : IInitializable, IDisposable
    {
        private readonly IAssetProvider _assetProvider;
        private AssetScope _assetScope;

        public ClickerLiveOpEntryPoint(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        
        public void Initialize()
        {
            _assetScope = new AssetScope(_assetProvider);
            RegisterLobbyIcon();
        }

        private void RegisterLobbyIcon()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _assetScope?.Dispose();
        }
    }
}