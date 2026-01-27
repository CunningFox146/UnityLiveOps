using System;
using System.Threading;
using App.Shared.Mvc.Factories;
using App.Shared.Utils;
using Cysharp.Threading.Tasks;

namespace App.Shared.Mvc.Services
{
    public class ControllerService : IControllerService
    {
        private readonly IControllerFactory _controllerFactory;

        public ControllerService(IControllerFactory controllerFactory)
        {
            _controllerFactory = controllerFactory;
        }

        public async UniTask StartController<T>(CancellationToken token = default) 
            where T : class, IController<Empty, Empty>
        {
            var controller = _controllerFactory.Create<T>();
            await controller.Start(Empty.Default, token);
            DisposeOnCancellation(controller, token).Forget();
        }
        
        public async UniTask StartController<T, TInput>(TInput input, CancellationToken token = default) 
            where T : class, IController<TInput, Empty>
        {
            var controller = _controllerFactory.Create<T>();
            await controller.Start(input, token);
            DisposeOnCancellation(controller, token).Forget();
        }

        public async UniTask<TResult> ShowViewWithResult<T, TResult>(CancellationToken token = default) 
            where T : class, IController<Empty, TResult>
        {
            using var controller = _controllerFactory.Create<T>();
            return await controller.Start(Empty.Default, token);
        }

        public async UniTask<TResult> ShowViewWithResult<T, TResult, TInput>(TInput input, CancellationToken token = default) 
            where T : class, IController<TInput, TResult>
        {
            using var controller = _controllerFactory.Create<T>();
            return await controller.Start(input, token);
        }

        private static async UniTaskVoid DisposeOnCancellation(IDisposable disposable, CancellationToken token)
        {
            await token.WaitUntilCanceled();
            disposable?.Dispose();
        }
    }
}