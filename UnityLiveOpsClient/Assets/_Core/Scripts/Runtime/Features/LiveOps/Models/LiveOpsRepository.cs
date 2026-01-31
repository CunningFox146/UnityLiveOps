using App.Shared.Logger;
using App.Shared.Repository;
using App.Shared.Storage;

namespace App.Runtime.Features.LiveOps.Models
{
    public class LiveOpsRepository : FeatureRepository<LiveOpsCalendar>
    {
        public LiveOpsRepository(IPersistentStorage persistentStorage, ILogger logger)
            : base(persistentStorage, logger, nameof(LiveOpsCalendar), LiveOpsCalendar.Empty)
        {
        }
    }
}