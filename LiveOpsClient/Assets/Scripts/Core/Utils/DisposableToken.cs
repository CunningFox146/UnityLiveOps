using System;
using System.Diagnostics.CodeAnalysis;

namespace CunningFox.Utils
{
    /// <summary>
    /// Disposable Token for keeping action to be executed on dispose.
    /// </summary>
    public class DisposableToken : IDisposable
    {
        private readonly Action _disposeAction;

        internal DisposableToken([NotNull] Action disposeAction)
        {
            _disposeAction = disposeAction ?? throw new ArgumentNullException(nameof(disposeAction));
        }

        public void Dispose()
        {
            _disposeAction.Invoke();
        }
    }
}