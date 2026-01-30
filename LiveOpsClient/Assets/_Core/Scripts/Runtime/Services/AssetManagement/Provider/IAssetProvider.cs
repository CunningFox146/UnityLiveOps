using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace App.Runtime.Services.AssetManagement.Provider
{
    public interface IAssetProvider
    {
        public UniTask Initialize(CancellationToken token);
        UniTask<T> LoadAsset<T>(string key, CancellationToken cancellationToken);
        UniTask<SceneInstance> LoadScene(string key, LoadSceneMode mode, CancellationToken token);
        UniTask UnloadScene(SceneInstance scene, CancellationToken token = default);
        void Release(object asset);
    }
}