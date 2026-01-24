using System;
using System.Threading;
using Common.Assets.Scripts.Common.Utils;
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

        public async UniTask ShowView<T>(CancellationToken token = default) 
            where T : class, IViewController<Empty, Empty>
        {
            var viewController = _controllerFactory.Create<T>();
            await viewController.Start(Empty.Default, token);
            DisposeOnCancellation(viewController, token).Forget();
        }
        
        public async UniTask ShowView<T, TInput>(TInput input, CancellationToken token = default) 
            where T : class, IViewController<TInput, Empty>
        {
            var viewController = _controllerFactory.Create<T>();
            await viewController.Start(input, token);
            DisposeOnCancellation(viewController, token).Forget();
        }

        public async UniTask<TResult> ShowViewWithResult<T, TResult>(CancellationToken token = default) 
            where T : class, IViewController<Empty, TResult>
        {
            using var viewController = _controllerFactory.Create<T>();
            return await viewController.Start(Empty.Default, token);
        }

        public async UniTask<TResult> ShowViewWithResult<T, TResult, TInput>(TInput input, CancellationToken token = default) 
            where T : class, IViewController<TInput, TResult>
        {
            using var viewController = _controllerFactory.Create<T>();
            return await viewController.Start(input, token);
        }

        private static async UniTaskVoid DisposeOnCancellation(IDisposable disposable, CancellationToken token)
        {
            await token.WaitUntilCanceled();
            disposable?.Dispose();
        }
    }
}