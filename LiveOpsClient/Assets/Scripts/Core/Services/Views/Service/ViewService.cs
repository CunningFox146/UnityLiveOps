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
        
        public async UniTask ShowView<T>(CancellationToken token = default) where T : class, IViewController
        {
            var viewController = _controllerFactory.Create<T>();
            await viewController.Start(token);
            await token.WaitUntilCanceled();
            viewController.Dispose();
        }

        public async UniTask ShowView<T, TInput>(TInput input, CancellationToken token = default) 
            where T : class, IViewControllerWithResult<EmptyControllerArg, TInput>
        {
            var viewController = _controllerFactory.Create<T, EmptyControllerArg, TInput>();
            await viewController.Start(input, token);
            await token.WaitUntilCanceled();
            viewController.Dispose();
        }
        
        public async UniTask<TResult> ShowView<T, TResult>(CancellationToken token = default) 
            where T : class, IViewControllerWithResult<TResult, EmptyControllerArg>
        {
            using var viewController = _controllerFactory.Create<T, TResult, EmptyControllerArg>();
            return await viewController.Start(new EmptyControllerArg(), token);
        }

        public async UniTask<TResult> ShowView<T, TResult, TInput>(TInput input, CancellationToken token = default) 
            where T : class, IViewControllerWithResult<TResult, TInput>
        {
            using var viewController = _controllerFactory.Create<T, TResult, TInput>();
            return await viewController.Start(input, token);
        }
    }
}