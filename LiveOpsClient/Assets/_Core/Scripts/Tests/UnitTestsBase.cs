using System.Threading;
using App.Shared.Utils;
using NUnit.Framework;

namespace App.Tests
{
    [TestFixture]
    public abstract class UnitTestsBase
    {
        private CancellationTokenSource _cancellationTokenSource;
        protected CancellationToken Token => _cancellationTokenSource.Token;

        [SetUp]
        public void SetUp()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            OnSetUp();
        }
		
        [TearDown]
        public void TearDown()
        {
            _cancellationTokenSource.CancelAndDispose();
            OnTearDown();
        }

        protected virtual void OnSetUp() { }
        protected virtual void OnTearDown() { }
    }
}