using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Assets.Scripts.AssetManagement.Provider
{
    public interface IAssetProvider
    {
        public UniTask InitializeAsync(CancellationToken cancellationToken);
        UniTask<T> LoadAssetAsync<T>(string key, CancellationToken cancellationToken = default) where T : Object;
        void Release(Object asset);
    }
}