using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Core.Services.Views
{
    public interface IViewController : IDisposable
    {
        UniTask Start(CancellationToken token);
    }

    public interface IViewControllerWithResult<TResult, in TInput> : IDisposable
    {
        UniTask<TResult> Start(TInput input, CancellationToken token);
    }
    
    public readonly struct EmptyControllerArg { }
}