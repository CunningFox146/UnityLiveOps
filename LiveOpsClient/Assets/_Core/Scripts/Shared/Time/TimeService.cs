using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace App.Shared.Time
{
    public class TimeService : IAsyncStartable, ITimeService
    {
        public event Action<DateTime> OnTimeChanged;
        public DateTime Now => DateTime.UtcNow;

        public UniTask StartAsync(CancellationToken cancellation = default)
        {
            StartTimeUpdate(cancellation).Forget();
            return UniTask.CompletedTask;
        }
        
        private async UniTaskVoid StartTimeUpdate(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                OnTimeChanged?.Invoke(Now);
                await UniTask.Delay(1000, cancellationToken: token);
            }
        }
    }
}