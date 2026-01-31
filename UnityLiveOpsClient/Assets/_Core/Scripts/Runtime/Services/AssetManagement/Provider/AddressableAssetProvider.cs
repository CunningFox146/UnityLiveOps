using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace App.Runtime.Services.AssetManagement.Provider
{
    public class AddressableAssetProvider : IAssetProvider, IDisposable
    {
        private readonly Dictionary<object, AsyncOperationHandle> _handleCache = new();

        public UniTask Initialize(CancellationToken token)
        {
            return Addressables.InitializeAsync().ToUniTask(cancellationToken: token);
        }

        public async UniTask<T> LoadAsset<T>(string key, CancellationToken cancellationToken)
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

        public async UniTask<SceneInstance> LoadScene(string key, LoadSceneMode mode,
            CancellationToken token)
        {
            var handle = Addressables.LoadSceneAsync(key, mode);
            try
            {
                return await handle.ToUniTask(cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                if (handle.IsValid())
                    Addressables.Release(handle);
                throw;
            }
        }

        public UniTask UnloadScene(SceneInstance scene, CancellationToken token = default)
            => Addressables.UnloadSceneAsync(scene).ToUniTask(cancellationToken: token);

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