using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Runtime.Services.AssetManagement.Provider
{
    public interface IAssetProvider
    {
        public UniTask InitializeAsync(CancellationToken cancellationToken);
        UniTask<T> LoadAssetAsync<T>(string key, CancellationToken cancellationToken = default);
        void Release(object asset);
    }
}