using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Core.Services.Views
{
    public class ViewControllerWithResult<TInput> : IViewControllerWithResult<EmptyControllerArg, TInput>
    {
        public async UniTask<EmptyControllerArg> Start(TInput input, CancellationToken token)
        {
            await StartFlow(input, token);
            return new EmptyControllerArg();
        }
        public void Dispose() => StopFlow();
        protected virtual UniTask StartFlow(TInput input, CancellationToken token) => UniTask.CompletedTask;
        protected virtual void StopFlow() { }
    }
    
    public class ViewControllerWithResult<TResult, TInput> : IViewControllerWithResult<TResult, TInput>
    {
        public UniTask<TResult> Start(TInput input, CancellationToken token) => StartFlow(input, token);
        public void Dispose() => StopFlow();
        protected virtual UniTask<TResult> StartFlow(TInput input, CancellationToken token) => UniTask.FromResult(default(TResult));
        protected virtual void StopFlow() { }
    }
}