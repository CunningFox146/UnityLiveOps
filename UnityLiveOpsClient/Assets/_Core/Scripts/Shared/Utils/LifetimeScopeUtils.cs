using System;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace App.Shared.Utils
{
    public static class LifetimeScopeUtils
    {
        public static async UniTask<LifetimeScope> CreateChildAsync(this LifetimeScope parent, IInstaller installer,
            string childScopeName = null)
        {
            var child = parent.CreateChild(installer, childScopeName);
            child.autoRun = false;
            await UniTask.RunOnThreadPool(() => child.Build());
            return child;
        }
        
        public static async UniTask<LifetimeScope> CreateChildAsync(this LifetimeScope parent, Action<IContainerBuilder> installer,
            string childScopeName = null)
        {
            var child = parent.CreateChild(installer, childScopeName);
            child.autoRun = false;
            await UniTask.RunOnThreadPool(() => child.Build());
            return child;
        }
    }
}