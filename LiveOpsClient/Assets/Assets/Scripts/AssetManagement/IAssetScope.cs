using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Core.Services.AssetProvider
{
    /// <summary>
    /// Scoped asset loading that tracks assets loaded within a feature.
    /// Disposing the scope releases only the assets loaded through it.
    /// </summary>
    public interface IAssetScope : IDisposable
    {
        UniTask<T> LoadAssetAsync<T>(string key, CancellationToken cancellationToken = default) where T : Object;
    }
}
