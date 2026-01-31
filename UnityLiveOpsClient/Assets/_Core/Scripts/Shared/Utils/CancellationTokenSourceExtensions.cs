using System.Threading;

namespace App.Shared.Utils
{
    public static class CancellationTokenSourceExtensions
    {
        public static void CancelAndDispose(this CancellationTokenSource cts)
        {
            cts.Cancel();
            cts.Dispose();
        }
    }
}