using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Core.Services.Views
{
    public interface IViewService
    {
        /// <summary>
        /// Shows a view with no input and no result (ViewControllerBase)
        /// </summary>
        UniTask ShowView<T>(CancellationToken token = default) where T : class, IViewController;

        /// <summary>
        /// Shows a view with no input but returns a result (ViewControllerBase&lt;TResult&gt;)
        /// </summary>
        UniTask<TResult> ShowView<T, TResult>(CancellationToken token = default) 
            where T : class, IViewControllerWithResult<TResult, EmptyControllerArg>;

        /// <summary>
        /// Shows a view with input but no result (ViewControllerWithResult&lt;TInput&gt;)
        /// </summary>
        UniTask ShowView<T, TInput>(TInput input, CancellationToken token = default) 
            where T : class, IViewControllerWithResult<EmptyControllerArg, TInput>;

        /// <summary>
        /// Shows a view with input and returns a result (ViewControllerWithResult&lt;TResult, TInput&gt;)
        /// </summary>
        UniTask<TResult> ShowView<T, TResult, TInput>(TInput input, CancellationToken token = default) 
            where T : class, IViewControllerWithResult<TResult, TInput>;
    }
}