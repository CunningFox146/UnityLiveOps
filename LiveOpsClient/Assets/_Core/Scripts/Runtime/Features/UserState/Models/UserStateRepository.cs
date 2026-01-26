using App.Shared.Logger;
using App.Shared.Repository;
using App.Shared.Storage;

namespace App.Runtime.Services.UserState
{
    public class UserStateRepository : FeatureRepository<ActiveUserState>
    {
        public UserStateRepository(IPersistentStorage persistentStorage, ILogger logger)
            : base(persistentStorage, logger, nameof(ActiveUserState), new ActiveUserState())
        {
        }
    }
}