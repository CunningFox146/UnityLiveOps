using System;
using System.Threading;
using Core.Infrastructure.Logger;
using Cysharp.Threading.Tasks;

namespace Core.Core.Services.Views
{
    public class ViewService : IViewService
    {
        private readonly IViewControllerFactory _controllerFactory;
        private readonly ILogger _logger;

        public ViewService(IViewControllerFactory controllerFactory, ILogger logger)
        {
            _controllerFactory = controllerFactory;
            _logger = logger;
        }
        
        public async UniTask ShowView<T>(CancellationToken token = default) where T : class, IViewController
        {
            var viewController = _controllerFactory.Create<T>();
            try
            {
                await viewController.Start(token);
                DisposeOnCancellation(token, viewController).Forget(_logger.LogUniTask);
            }
            finally
            {
                viewController?.Dispose();
            }
        }

        public async UniTask ShowView<T, TInput>(TInput input, CancellationToken token = default) 
            where T : class, IViewControllerWithResult<EmptyControllerArg, TInput>
        {
            var viewController = _controllerFactory.Create<T, EmptyControllerArg, TInput>();
            try
            {
                await viewController.Start(input, token);
                DisposeOnCancellation(token, viewController).Forget(_logger.LogUniTask);
            }
            finally
            {
                viewController?.Dispose();
            }
        }
        
        public async UniTask<TResult> ShowView<T, TResult>(CancellationToken token = default) 
            where T : class, IViewControllerWithResult<TResult, EmptyControllerArg>
        {
            using var viewController = _controllerFactory.Create<T, TResult, EmptyControllerArg>();
            DisposeOnCancellation(token, viewController).Forget(_logger.LogUniTask);
            return await viewController.Start(new EmptyControllerArg(), token);
        }

        public async UniTask<TResult> ShowView<T, TResult, TInput>(TInput input, CancellationToken token = default) 
            where T : class, IViewControllerWithResult<TResult, TInput>
        {
            using var viewController = _controllerFactory.Create<T, TResult, TInput>();
            DisposeOnCancellation(token, viewController).Forget(_logger.LogUniTask);
            return await viewController.Start(input, token);
        }
        
        private static async UniTask DisposeOnCancellation<T>(CancellationToken token, T viewController)
            where T : IDisposable
        {
            await token.WaitUntilCanceled();
            viewController?.Dispose();
        }
    }
}