using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace App.Shared.Utils
{
    public static class LifetimeScopeUtils
    {
        public static async UniTask<LifetimeScope> CreateChildAsync<TScope>(this LifetimeScope parent, IInstaller installer,
            string childScopeName = null)
        {
            var child = parent.CreateChild(installer, childScopeName);
            child.autoRun = false;
            await UniTask.RunOnThreadPool(() => child.Build());
            return child;
        }
    }
}