using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Services.AssetProvider
{
    public class ResourcesAssetProvider : IAssetProvider
    {
        public async UniTask<T> LoadPrefab<T>(string path, CancellationToken token) where T : Object
        {
            var prefab = await Resources.LoadAsync<T>(path).ToUniTask(cancellationToken: token);
            return prefab as T;
        }
    }
}