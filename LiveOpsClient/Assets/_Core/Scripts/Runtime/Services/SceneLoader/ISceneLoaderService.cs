using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace App.Runtime.Services.SceneLoader
{
    public interface ISceneLoaderService
    {
        UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, CancellationToken cancellationToken = default);
        UniTask LoadSceneAsync(string sceneName, IProgress<float> progress, LoadSceneMode mode,
            CancellationToken cancellationToken = default);
        UniTask UnloadSceneAsync(string sceneName, CancellationToken cancellationToken = default);
        UniTask UnloadSceneAsync(string sceneName, UnloadSceneOptions options, CancellationToken cancellationToken = default);
    }
}