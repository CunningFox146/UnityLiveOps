using App.Runtime.Features.Common.Models;
using VContainer.Unity;

namespace App.Runtime.Features.Common.Services
{
    public interface IFeatureService
    {
        void StartFeature(FeatureType featureType, IInstaller installer);
        void StopFeature(FeatureType featureType);
        bool IsFeatureActive(FeatureType featureType);
    }
}