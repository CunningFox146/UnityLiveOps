using System;
using System.Collections.Generic;
using System.Threading;
using App.Shared.Logger;
using App.Shared.Utils;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace App.Runtime.Features.Common
{
    public class FeatureService : IFeatureService
    {
        private readonly LifetimeScope _lifetimeScope;
        private readonly ILogger _logger;
        private readonly Dictionary<FeatureType, LifetimeScope> _scopes;

        public FeatureService(LifetimeScope lifetimeScope, ILogger logger)
        {
            _lifetimeScope = lifetimeScope;
            _logger = logger;
        }

        public async UniTask StartFeature(FeatureType featureType, IInstaller featureInstaller, CancellationToken token)
        {
            if (_scopes.ContainsKey(featureType))
                return;

            try
            {
                var scope = await _lifetimeScope.CreateChildAsync<LifetimeScope>(featureInstaller);
                _scopes[featureType] = scope;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to install feature {featureType}.", ex);
            }
        }

        public void StopFeature(FeatureType featureType)
        {
            if (!_scopes.Remove(featureType, out var scope))
                return;
            scope.Dispose();
        }
    }
}