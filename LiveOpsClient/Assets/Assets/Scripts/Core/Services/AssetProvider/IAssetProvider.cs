using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Services.AssetProvider
{
    public interface IAssetProvider
    {
        UniTask<T> LoadPrefab<T>(string path, CancellationToken token) where T : Object;
    }
}