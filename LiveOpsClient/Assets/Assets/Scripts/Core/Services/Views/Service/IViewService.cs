using System.Threading;
using Common.Assets.Scripts.Common.Utils;
using Cysharp.Threading.Tasks;

namespace Core.Core.Services.Views
{
    public interface IViewService
    {
        /// <summary>
        /// Shows a view with no input and no result.
        /// </summary>
        UniTask ShowView<T>(CancellationToken token = default) 
            where T : class, IViewController<Empty, Empty>;

        /// <summary>
        /// Shows a view with input but no result.
        /// </summary>
        UniTask ShowView<T, TInput>(TInput input, CancellationToken token = default) 
            where T : class, IViewController<TInput, Empty>;

        /// <summary>
        /// Shows a view and returns a result (no input).
        /// </summary>
        UniTask<TResult> ShowViewWithResult<T, TResult>(CancellationToken token = default) 
            where T : class, IViewController<Empty, TResult>;

        /// <summary>
        /// Shows a view with input and returns a result.
        /// </summary>
        UniTask<TResult> ShowViewWithResult<T, TResult, TInput>(TInput input, CancellationToken token = default) 
            where T : class, IViewController<TInput, TResult>;
    }
}