using System;
using VContainer;
using VContainer.Unity;

namespace App.Runtime.Features.Common
{
    public interface IFeatureService
    {
        void StartFeature(FeatureType featureType, IInstaller installer);
        void StopFeature(FeatureType featureType);
        bool IsFeatureActive(FeatureType featureType);
    }
}