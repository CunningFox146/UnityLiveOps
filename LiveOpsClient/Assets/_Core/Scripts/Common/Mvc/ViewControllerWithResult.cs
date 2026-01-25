using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.Mvc
{
    /// <summary>
    /// View controller with input and result.
    /// Override OnStart(TInput) and OnStop().
    /// </summary>
    public abstract class ViewControllerWithResult<TResult, TInput> : IViewController<TInput, TResult>
    {
        public UniTask<TResult> Start(TInput input, CancellationToken token) => OnStart(input, token);

        public void Dispose() => OnStop();

        protected virtual UniTask<TResult> OnStart(TInput input, CancellationToken token) 
            => UniTask.FromResult(default(TResult));
        protected virtual void OnStop() { }
    }
}