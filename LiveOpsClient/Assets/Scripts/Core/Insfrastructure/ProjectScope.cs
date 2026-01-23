using Core.Infrastructure.Logger;
using VContainer;
using VContainer.Unity;

namespace CunningFox
{
    public class ProjectScope : LifetimeScope
    {
        private void OnEnable()
            => DontDestroyOnLoad(this);

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ILogger, UnityLogger>(Lifetime.Singleton);
            builder.RegisterEntryPoint<Boot>();
        }
    }
}
