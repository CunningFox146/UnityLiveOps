using System;
using System.Net.Http;
using CunningFox.LiveOps.Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace CunningFox
{
    public class Test : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<TestEntryPoint>();
        }

        private void LateUpdate()
        {
            Container.Resolve<Cool>();
        }
    }

    public class TestEntryPoint : IStartable, IDisposable
    {
        private readonly LifetimeScope _rootScope;
        private LifetimeScope _child;

        public TestEntryPoint(LifetimeScope rootScope)
        {
            _rootScope = rootScope;
        }
        
        public void Start()
        {
            _child = _rootScope.CreateChild(new TestInstaller());
        }

        public void Dispose()
        {
            _child?.Dispose();
        }
    }

    public class TestInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<Cool>(Lifetime.Scoped);
        }
    }

    public class Cool
    {
        
    }
}