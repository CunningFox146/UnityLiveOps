using System.Threading;
using Common.Utils;
using Cysharp.Threading.Tasks;

namespace Common.Mvc
{
    /// <summary>
    /// View controller with result but no input.
    /// Override OnStart() and OnStop().
    /// </summary>
    public abstract class ViewControllerWithResult<TResult> : IViewController<Empty, TResult>
    {
        public UniTask<TResult> Start(Empty input, CancellationToken token) => OnStart(token);

        public void Dispose() => OnStop();

        protected virtual UniTask<TResult> OnStart(CancellationToken token) 
            => UniTask.FromResult(default(TResult));
        protected virtual void OnStop() { }
    }
}