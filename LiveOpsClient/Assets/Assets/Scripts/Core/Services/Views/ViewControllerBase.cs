using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Core.Services.Views
{
    /// <summary>
    /// View controller with no input and no result.
    /// Override OnStart() and OnStop().
    /// </summary>
    public abstract class ViewControllerBase : IViewController<Empty, Empty>
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

    /// <summary>
    /// View controller with input but no result.
    /// Override OnStart(TInput) and OnStop().
    /// </summary>
    public abstract class ViewControllerBase<TInput> : IViewController<TInput, Empty>
    {
        public async UniTask<Empty> Start(TInput input, CancellationToken token)
        {
            await OnStart(input, token);
            return Empty.Default;
        }

        public void Dispose() => OnStop();

        protected virtual UniTask OnStart(TInput input, CancellationToken token) => UniTask.CompletedTask;
        protected virtual void OnStop() { }
    }

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