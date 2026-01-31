using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Shared.Mvc
{
    /// <summary>
    /// Unified view controller interface with input and result types.
    /// Use Empty for TResult or TInput when not needed.
    /// </summary>
    public interface IController<in TInput, TResult> : IDisposable
    {
        UniTask<TResult> Start(TInput input, CancellationToken token);
    }
}