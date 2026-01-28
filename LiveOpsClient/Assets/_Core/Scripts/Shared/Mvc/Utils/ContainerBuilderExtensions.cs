using App.Shared.Mvc.Factories;
using App.Shared.Mvc.Services;
using VContainer;

namespace App.Shared.Mvc.Utils
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterControllerServices(this IContainerBuilder builder)
        {
            builder.Register<IControllerFactory, ControllerFactory>(Lifetime.Scoped);
            builder.Register<IControllerService, ControllerService>(Lifetime.Scoped);
        }

        public static RegistrationBuilder RegisterController<TController>(this IContainerBuilder builder)
            => builder.Register<TController>(Lifetime.Scoped);
    }
}