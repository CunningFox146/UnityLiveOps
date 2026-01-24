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

        public T Create<T, TResult, TInput>() where T : class, IViewController<TResult, TInput>
        {
            return _container.Resolve<T>();
        }
    }
}