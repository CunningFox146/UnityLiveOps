using App.Shared.Logger;
using App.Shared.Repository;
using App.Shared.Storage;

namespace App.Runtime.Features.ClickerLiveOp.Model
{
    public class ClickerLiveOpRepository : FeatureRepository<ClickerLiveOpState>
    {
        public ClickerLiveOpRepository(IPersistentStorage persistentStorage, ILogger logger)
            : base(persistentStorage, logger, nameof(ClickerLiveOpState), new  ClickerLiveOpState())
        {
        }
    }
}