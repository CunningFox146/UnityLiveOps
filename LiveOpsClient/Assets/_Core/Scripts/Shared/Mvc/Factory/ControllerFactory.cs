using VContainer;

namespace App.Shared.Mvc.Factory
{
    public class ControllerFactory : IControllerFactory
    {
        private readonly IObjectResolver _container;

        public ControllerFactory(IObjectResolver container)
        {
            _container = container;
        }

        public T Create<T>() where T : class
        {
            return _container.Resolve<T>();
        }
    }
}