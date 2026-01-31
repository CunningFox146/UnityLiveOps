using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace App.Shared.Time
{
    public class TimeService : IAsyncStartable, ITimeService
    {
        public event Action<DateTime> TimeChanged;
        public DateTime Now => DateTime.UtcNow;

        public UniTask StartAsync(CancellationToken token = default)
        {
            StartTimeUpdate(token).Forget();
            return UniTask.CompletedTask;
        }
        
        private async UniTaskVoid StartTimeUpdate(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                TimeChanged?.Invoke(Now);
                await UniTask.Delay(1000, cancellationToken: token);
            }
        }
    }
}