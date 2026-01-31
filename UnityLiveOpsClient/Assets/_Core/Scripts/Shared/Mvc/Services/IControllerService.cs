using System.Threading;
using App.Shared.Utils;
using Cysharp.Threading.Tasks;

namespace App.Shared.Mvc.Services
{
    public interface IControllerService
    {
        /// <summary>
        /// Shows a view with no input and no result.
        /// </summary>
        UniTask StartController<T>(CancellationToken token = default) 
            where T : class, IController<Empty, Empty>;

        /// <summary>
        /// Shows a view with input but no result.
        /// </summary>
        UniTask StartController<T, TInput>(TInput input, CancellationToken token = default) 
            where T : class, IController<TInput, Empty>;

        /// <summary>
        /// Shows a view and returns a result (no input).
        /// </summary>
        UniTask<TResult> StartControllerWithResult<T, TResult>(CancellationToken token = default) 
            where T : class, IController<Empty, TResult>;

        /// <summary>
        /// Shows a view with input and returns a result.
        /// </summary>
        UniTask<TResult> StartControllerWithResult<T, TInput, TResult>(TInput input, CancellationToken token = default) 
            where T : class, IController<TInput, TResult>;
    }
}