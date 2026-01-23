using System;
using Core.Infrastructure.Logger;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace CunningFox
{
    public class Boot : IStartable
    {
        private readonly ILogger _logger;

        public Boot(ILogger logger)
        {
            _logger = logger;
        }
        
        public void Start()
        {
            
        }
    }
}