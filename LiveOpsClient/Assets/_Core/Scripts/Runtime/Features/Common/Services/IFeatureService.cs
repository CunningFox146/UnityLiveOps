using System;
using VContainer;

namespace App.Runtime.Features.Common
{
    public interface IFeatureService
    {
        void StartFeature(FeatureType featureType, Action<IContainerBuilder> installation);
        void StopFeature(FeatureType featureType);
    }
}