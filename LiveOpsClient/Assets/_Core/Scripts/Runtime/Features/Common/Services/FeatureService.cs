using System;
using System.Collections.Generic;
using App.Runtime.Features.Common.Models;
using App.Shared.Logger;
using VContainer.Unity;

namespace App.Runtime.Features.Common.Services
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

        public void StartFeature(FeatureType featureType, IInstaller installer)
        {
            if (IsFeatureActive(featureType))
            {
                _logger.Error($"Feature {featureType} is already active");
                return;
            }
            try
            {
                var scope = _lifetimeScope.CreateChild(installer, $"DI {featureType}");
                _scopes[featureType] = scope;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to install feature {featureType}", ex);
            }
        }

        public void StopFeature(FeatureType featureType)
        {
            if (!_scopes.Remove(featureType, out var scope))
                return;
            scope.Dispose();
        }
        
        public bool IsFeatureActive(FeatureType featureType)
            => _scopes.ContainsKey(featureType);
    }
}