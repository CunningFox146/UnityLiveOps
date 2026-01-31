using App.Shared.Logger;
using App.Shared.Repository;
using App.Shared.Storage;

namespace App.Runtime.Features.KeyCollectLiveOp.Model
{
    public class KeyCollectLiveOpRepository : FeatureRepository<KeyCollectLiveOpData>
    {
        public KeyCollectLiveOpRepository(IPersistentStorage persistentStorage, ILogger logger)
            : base(persistentStorage, logger, nameof(KeyCollectLiveOpData), new KeyCollectLiveOpData())
        {
        }
    }
}
