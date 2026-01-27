using System;
using System.Collections.Generic;
using System.Threading;
using App.Shared.Logger;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace App.Runtime.Features.Common
{
    public class FeatureService : IFeatureService
    {
        private readonly LifetimeScope _lifetimeScope;
        private readonly ILogger _logger;
        private readonly Dictionary<FeatureType, LifetimeScope> _scopes = new();

        public FeatureService(LifetimeScope lifetimeScope, ILogger logger)
        {
            _lifetimeScope = lifetimeScope;
            _logger = logger;
        }

        public UniTask StartFeature(FeatureType featureType, IInstaller featureInstaller, CancellationToken token)
        {
            if (_scopes.ContainsKey(featureType))
                return UniTask.CompletedTask;

            try
            {
                var scope = _lifetimeScope.CreateChild(featureInstaller);
                scope.name = $"DI {featureType}";
                _scopes[featureType] = scope;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to install feature {featureType}.", ex);
            }
            
            return UniTask.CompletedTask;
        }

        public void StopFeature(FeatureType featureType)
        {
            if (!_scopes.Remove(featureType, out var scope))
                return;
            scope.Dispose();
        }
    }
}