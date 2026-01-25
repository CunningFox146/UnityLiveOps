using System.Threading;
using Common.Utils;
using Cysharp.Threading.Tasks;

namespace Common.Mvc
{
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
}