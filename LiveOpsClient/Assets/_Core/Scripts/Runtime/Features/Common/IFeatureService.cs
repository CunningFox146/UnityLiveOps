using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace App.Runtime.Features.Common
{
    public interface IFeatureService
    {
        UniTask StartFeature(FeatureType featureType, IInstaller featureInstaller, CancellationToken token);
        void StopFeature(FeatureType featureType);
    }
}