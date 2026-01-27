using System.Threading;
using App.Shared.Utils;
using Cysharp.Threading.Tasks;

namespace App.Shared.Mvc
{
    /// <summary>
    /// View controller with no input and no result.
    /// Override OnStart() and OnStop().
    /// </summary>
    public abstract class ControllerBase : IController<Empty, Empty>
    {
        public async UniTask<Empty> Start(Empty input, CancellationToken token)
        {
            await OnStart(token);
            return Empty.Default;
        }

        public void Dispose() => OnStop();

        protected virtual UniTask OnStart(CancellationToken token) => UniTask.CompletedTask;
        protected virtual void OnStop() { }
    }
}