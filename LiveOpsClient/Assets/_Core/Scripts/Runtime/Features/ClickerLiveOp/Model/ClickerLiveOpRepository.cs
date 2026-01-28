using App.Shared.Logger;
using App.Shared.Repository;
using App.Shared.Storage;

namespace App.Runtime.Features.ClickerLiveOp.Model
{
    public class ClickerLiveOpRepository : FeatureRepository<ClickerLiveOpData>
    {
        public ClickerLiveOpRepository(IPersistentStorage persistentStorage, ILogger logger)
            : base(persistentStorage, logger, nameof(ClickerLiveOpData), new ClickerLiveOpData())
        {
        }
    }
}