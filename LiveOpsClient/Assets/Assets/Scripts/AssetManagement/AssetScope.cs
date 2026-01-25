using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Core.Services.AssetProvider
{
    /// <summary>
    /// Tracks assets loaded within a specific feature/context.
    /// When disposed, releases only the assets loaded through this scope.
    /// </summary>
    public class AssetScope : IAssetScope
    {
        private readonly IAssetProvider _provider;
        private readonly List<Object> _loadedAssets = new();
        private bool _disposed;

        public AssetScope(IAssetProvider provider)
        {
            _provider = provider;
        }

        public async UniTask<T> LoadAssetAsync<T>(string key, CancellationToken cancellationToken = default) where T : Object
        {
            ThrowIfDisposed();
            
            var asset = await _provider.LoadAssetAsync<T>(key, cancellationToken);
            _loadedAssets.Add(asset);
            return asset;
        }
        
        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            
            foreach (var asset in _loadedAssets)
                if (asset != null)
                    _provider.Release(asset);
            
            _loadedAssets.Clear();
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(AssetScope));
        }
    }
}
