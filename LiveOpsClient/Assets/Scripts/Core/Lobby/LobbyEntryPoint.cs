using System;
using System.Threading;
using CunningFox.AssetProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Core.Lobby
{
    public class LobbyEntryPoint : IAsyncStartable, IDisposable
    {
        private readonly IAssetProvider _assetProvider;

        public LobbyEntryPoint(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        
        public async UniTask StartAsync(CancellationToken token = default)
        {
            var prefab = await _assetProvider.LoadPrefab<Canvas>("Views/BaseView", token);
            var instance = Object.Instantiate(prefab);
            
        }
        
        public void Dispose()
        {
            
        }
    }
}