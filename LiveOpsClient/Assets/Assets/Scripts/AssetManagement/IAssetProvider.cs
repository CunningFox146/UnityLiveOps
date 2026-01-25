using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Services.AssetProvider
{
    public interface IAssetProvider
    {
        public UniTask InitializeAsync(CancellationToken cancellationToken);
        UniTask<T> LoadAssetAsync<T>(string key, CancellationToken cancellationToken = default) where T : Object;
        void Release(Object asset);
    }
}