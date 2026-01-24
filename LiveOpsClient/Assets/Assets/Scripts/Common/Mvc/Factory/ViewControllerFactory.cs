using VContainer;

namespace Common.Mvc.Factory
{
    public class ViewControllerFactory : IViewControllerFactory
    {
        private readonly IObjectResolver _container;

        public ViewControllerFactory(IObjectResolver container)
        {
            _container = container;
        }

        public T Create<T>() where T : class
        {
            return _container.Resolve<T>();
        }
    }
}