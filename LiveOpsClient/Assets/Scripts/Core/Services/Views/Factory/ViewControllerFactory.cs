using VContainer;

namespace Core.Core.Services.Views
{
    public class ViewControllerFactory : IViewControllerFactory
    {
        private readonly IObjectResolver _container;

        public ViewControllerFactory(IObjectResolver container)
        {
            _container = container;
        }
        
        public T Create<T>() where T : class, IViewController
        {
            return _container.Resolve<T>();
        }
        
        public T Create<T, TResult, TInput>() where T : class, IViewControllerWithResult<TResult, TInput>
        {
            return _container.Resolve<T>();
        }
    }
}