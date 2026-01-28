using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace App.Runtime.Services.AssetManagement.Provider
{
    public class AddressableAssetProvider : IAssetProvider, IDisposable
    {
        private readonly Dictionary<object, AsyncOperationHandle> _handleCache = new();

        public UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            return Addressables.InitializeAsync().ToUniTask(cancellationToken: cancellationToken);
        }

        public async UniTask<T> LoadAssetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var handle = Addressables.LoadAssetAsync<T>(key);
            try
            {
                var result = await handle.ToUniTask(cancellationToken: cancellationToken);
                _handleCache[result] = handle;
                return result;
            }
            catch (OperationCanceledException)
            {
                if (handle.IsValid())
                    Addressables.Release(handle);
                throw;
            }
        }

        public void Release(object asset)
        {
            if (_handleCache.Remove(asset, out var handle) && handle.IsValid())
                Addressables.Release(handle);
        }

        public void Dispose()
        {
            foreach (var handle in _handleCache.Values)
                if (handle.IsValid())
                    Addressables.Release(handle);
            
            _handleCache.Clear();
        }
    }
}