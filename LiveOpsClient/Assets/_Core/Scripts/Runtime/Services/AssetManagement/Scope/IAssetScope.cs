using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Runtime.Services.AssetManagement.Scope
{
    /// <summary>
    /// Scoped asset loading that tracks assets loaded within a feature.
    /// Disposing the scope releases only the assets loaded through it.
    /// </summary>
    public interface IAssetScope : IDisposable
    {
        UniTask<GameObject> InstantiateAsync(string key, Transform parent = null,
            CancellationToken cancellationToken = default);
        
        UniTask<TComponent> InstantiateAsync<TComponent>(string key, Transform parent = null, CancellationToken cancellationToken = default)
            where TComponent : Component;

        UniTask<T> LoadAssetAsync<T>(string key, CancellationToken token = default) where T : Object;
    }
}