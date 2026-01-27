using System.Threading;
using App.Shared.Utils;
using Cysharp.Threading.Tasks;

namespace App.Shared.Mvc
{
    /// <summary>
    /// View controller with result but no input.
    /// Override OnStart() and OnStop().
    /// </summary>
    public abstract class ControllerWithResult<TResult> : IController<Empty, TResult>
    {
        public UniTask<TResult> Start(Empty input, CancellationToken token) => OnStart(token);

        public void Dispose() => OnStop();

        protected virtual UniTask<TResult> OnStart(CancellationToken token) 
            => UniTask.FromResult(default(TResult));
        protected virtual void OnStop() { }
    }
}