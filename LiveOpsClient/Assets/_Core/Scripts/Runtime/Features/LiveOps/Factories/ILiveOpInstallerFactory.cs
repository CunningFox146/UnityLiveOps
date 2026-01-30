using App.Runtime.Features.Common.Models;
using App.Runtime.Features.LiveOps.Models;
using VContainer.Unity;

namespace App.Runtime.Features.LiveOps.Factories
{
    public interface ILiveOpInstallerFactory
    {
        IInstaller CreateInstaller(LiveOpState state);
    }
}