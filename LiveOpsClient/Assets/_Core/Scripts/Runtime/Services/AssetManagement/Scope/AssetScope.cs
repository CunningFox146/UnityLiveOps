using System;
using System.Collections.Generic;
using System.Threading;
using App.Runtime.Services.AssetManagement.Provider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Runtime.Services.AssetManagement.Scope
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

        public async UniTask<GameObject> InstantiateAsync(string key,
            Transform parent = null,
            CancellationToken token = default)
        {
            var asset = await LoadAssetAsync<GameObject>(key, token);
            return Object.Instantiate(asset, parent);
        }

        public async UniTask<TComponent> InstantiateAsync<TComponent>(string key,
            Transform parent = null,
            CancellationToken token = default)
            where TComponent : Component
        {
            var obj = await InstantiateAsync(key, parent, token);
            return obj.TryGetComponent(out TComponent component) ? component : null;
        }

        public async UniTask<T> LoadAssetAsync<T>(string key, CancellationToken token = default)
            where T : Object
        {
            ThrowIfDisposed();

            var asset = await _provider.LoadAssetAsync<T>(key, token);
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