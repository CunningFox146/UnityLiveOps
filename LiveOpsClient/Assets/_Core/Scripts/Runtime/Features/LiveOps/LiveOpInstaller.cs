using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.LiveOps
{
    public class LiveOpInstaller : IInstaller
    {
        public static IInstaller Default => new LiveOpInstaller();
        
        public void Install(IContainerBuilder builder)
        {
            
        }
    }
}