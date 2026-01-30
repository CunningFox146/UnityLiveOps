using System.Collections.Generic;
using System.Threading;
using App.Runtime.Services.AssetManagement.Provider;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace App.Runtime.Services.SceneLoader
{
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly IAssetProvider _assetProvider;
        private readonly Dictionary<string, SceneInstance> _activeScenes = new();

        public SceneLoaderService(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask<Scene> LoadScene(string sceneName,
            CancellationToken cancellationToken, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (_activeScenes.TryGetValue(sceneName, out var activeScene))
                return activeScene.Scene;
            
            var sceneInstance = await _assetProvider.LoadScene(sceneName, mode, cancellationToken);
            _activeScenes[sceneName] = sceneInstance;
            return sceneInstance.Scene;
        }

        public UniTask UnloadScene(string sceneName, CancellationToken cancellationToken)
            => _activeScenes.TryGetValue(sceneName, out var activeScene)
                ? _assetProvider.UnloadScene(activeScene, cancellationToken)
                : throw new KeyNotFoundException($"Scene '{sceneName}' was not found.");

        public UniTask LoadBuiltinScene(string sceneName,
            CancellationToken cancellationToken, LoadSceneMode mode = LoadSceneMode.Single)
            => SceneManager.LoadSceneAsync(sceneName, mode).ToUniTask(cancellationToken: cancellationToken);

        public UniTask UnloadBuiltinScene(string sceneName, CancellationToken cancellationToken)
            => SceneManager.UnloadSceneAsync(sceneName).ToUniTask(cancellationToken: cancellationToken);
    }
}