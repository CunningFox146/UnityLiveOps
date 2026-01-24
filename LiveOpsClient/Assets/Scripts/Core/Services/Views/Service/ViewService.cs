using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Core.Services.Views
{
    public class ViewService : IViewService
    {
        private readonly IViewControllerFactory _controllerFactory;

        public ViewService(IViewControllerFactory controllerFactory)
        {
            _controllerFactory = controllerFactory;
        }

        /// <summary>
        /// Shows a view that lives until the cancellation token is cancelled.
        /// Returns immediately after Start completes.
        /// </summary>
        public async UniTask ShowView<T>(CancellationToken token = default) 
            where T : class, IViewController<Empty, Empty>
        {
            var viewController = _controllerFactory.Create<T, Empty, Empty>();
            await viewController.Start(Empty.Default, token);
            DisposeOnCancellation(viewController, token).Forget();
        }

        /// <summary>
        /// Shows a view with input that lives until the cancellation token is cancelled.
        /// Returns immediately after Start completes.
        /// </summary>
        public async UniTask ShowView<T, TInput>(TInput input, CancellationToken token = default) 
            where T : class, IViewController<Empty, TInput>
        {
            var viewController = _controllerFactory.Create<T, Empty, TInput>();
            await viewController.Start(input, token);
            DisposeOnCancellation(viewController, token).Forget();
        }

        /// <summary>
        /// Shows a view and waits for it to return a result. Disposed after returning.
        /// </summary>
        public async UniTask<TResult> ShowViewWithResult<T, TResult>(CancellationToken token = default) 
            where T : class, IViewController<TResult, Empty>
        {
            using var viewController = _controllerFactory.Create<T, TResult, Empty>();
            return await viewController.Start(Empty.Default, token);
        }

        /// <summary>
        /// Shows a view with input and waits for it to return a result. Disposed after returning.
        /// </summary>
        public async UniTask<TResult> ShowViewWithResult<T, TResult, TInput>(TInput input, CancellationToken token = default) 
            where T : class, IViewController<TResult, TInput>
        {
            using var viewController = _controllerFactory.Create<T, TResult, TInput>();
            return await viewController.Start(input, token);
        }

        private static async UniTaskVoid DisposeOnCancellation(IDisposable disposable, CancellationToken token)
        {
            await token.WaitUntilCanceled();
            disposable?.Dispose();
        }
    }
}