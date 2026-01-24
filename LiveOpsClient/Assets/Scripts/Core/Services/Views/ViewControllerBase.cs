using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Core.Services.Views
{
    public class ViewControllerBase : IViewController
    {
        public UniTask Start(CancellationToken token) => StartFlow(token);
        public void Dispose() => StopFlow();
        protected virtual UniTask StartFlow(CancellationToken token) => UniTask.CompletedTask;
        protected virtual void StopFlow() { }
    }

    public class ViewControllerBase<TResult> : IViewControllerWithResult<TResult, EmptyControllerArg>
    {
        public UniTask<TResult> Start(EmptyControllerArg input, CancellationToken token) => StartFlow(token);
        public void Dispose() => StopFlow();
        protected virtual UniTask<TResult> StartFlow(CancellationToken token) => UniTask.FromResult(default(TResult));
        protected virtual void StopFlow() { }
    }
}