using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Core.Services.Views
{
    /// <summary>
    /// Unified view controller interface with input and result types.
    /// Use Empty for TResult or TInput when not needed.
    /// </summary>
    public interface IViewController<in TInput, TResult> : IDisposable
    {
        UniTask<TResult> Start(TInput input, CancellationToken token);
    }
}