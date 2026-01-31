using App.Shared.Logger;
using App.Shared.Repository;
using App.Shared.Storage;

namespace App.Runtime.Features.PlayGamesLiveOp.Model
{
    public class PlayGamesLiveOpRepository : FeatureRepository<PlayGamesLiveOpData>
    {
        public PlayGamesLiveOpRepository(IPersistentStorage persistentStorage, ILogger logger)
            : base(persistentStorage, logger, nameof(PlayGamesLiveOpData), new PlayGamesLiveOpData())
        {
        }
    }
}
