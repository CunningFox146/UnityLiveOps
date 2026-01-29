using System;
using App.Runtime.Features.ClickerLiveOp;
using App.Runtime.Features.ClickerLiveOp.Model;
using App.Runtime.Features.Common;
using App.Runtime.Features.Common.Models;
using App.Runtime.Features.LiveOps.Models;
using VContainer.Unity;

namespace App.Runtime.Features.LiveOps
{
    public static class LiveOpInstallersPerFeature
    {
        public static IInstaller GetInstaller(LiveOpState state)
            => state.Type switch
            {
                FeatureType.ClickerLiveOp => new ClickerLiveOpInstaller(state),
                FeatureType.KeyCollectLiveOp => throw new NotImplementedException(),
                FeatureType.PlayGamesLiveOp => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(state.Type), state.Type, null)
            };
    }
}