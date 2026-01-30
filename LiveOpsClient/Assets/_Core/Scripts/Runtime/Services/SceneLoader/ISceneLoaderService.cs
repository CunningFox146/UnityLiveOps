using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace App.Runtime.Services.SceneLoader
{
    public interface ISceneLoaderService
    {
        UniTask<Scene> LoadScene(string sceneName,
            CancellationToken cancellationToken, LoadSceneMode mode = LoadSceneMode.Single);

        UniTask UnloadScene(string sceneName, CancellationToken cancellationToken);

        UniTask LoadBuiltinScene(string sceneName, CancellationToken cancellationToken,
            LoadSceneMode mode = LoadSceneMode.Single);

        UniTask UnloadBuiltinScene(string sceneName, CancellationToken cancellationToken);
        UniTask UnloadAllScenes(CancellationToken cancellationToken);
    }
}