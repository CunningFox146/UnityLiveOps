using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Common.SceneLoader
{
    public class SceneLoaderService : ISceneLoaderService
    {
        public async UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single,
            CancellationToken cancellationToken = default)
        {
            await SceneManager.LoadSceneAsync(sceneName, mode).ToUniTask(cancellationToken: cancellationToken);
        }

        public async UniTask LoadSceneAsync(string sceneName, IProgress<float> progress,
            LoadSceneMode mode = LoadSceneMode.Single,
            CancellationToken cancellationToken = default)
        {
            await SceneManager.LoadSceneAsync(sceneName, mode)
                .ToUniTask(progress, cancellationToken: cancellationToken);
        }

        public async UniTask UnloadSceneAsync(string sceneName, CancellationToken cancellationToken = default)
        {
            await SceneManager.UnloadSceneAsync(sceneName).ToUniTask(cancellationToken: cancellationToken);
        }

        public async UniTask UnloadSceneAsync(string sceneName, UnloadSceneOptions options,
            CancellationToken cancellationToken = default)
        {
            await SceneManager.UnloadSceneAsync(sceneName, options).ToUniTask(cancellationToken: cancellationToken);
        }
    }
}