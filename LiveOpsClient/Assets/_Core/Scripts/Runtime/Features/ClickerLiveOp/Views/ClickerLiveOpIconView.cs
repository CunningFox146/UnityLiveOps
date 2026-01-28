using System;
using System.Threading;
using App.Runtime.Features.Common.Views;
using App.Shared.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Runtime.Features.ClickerLiveOp.Views
{
    public class ClickerLiveOpIconView : EventIconView
    {
        [SerializeField] private int _animationPlayPeriod;
        [SerializeField] private Animation _animation;
        private CancellationTokenSource _animationCts;

        private void Start()
        {
            _animationCts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
            ScheduleAnimation(_animationCts.Token).Forget();
        }

        private void OnDestroy()
        {
            _animationCts?.Dispose();
        }

        public override void Expire()
        {
            base.Expire();
            _animation.enabled = false;
            _animationCts?.Cancel();
            _animationCts?.Dispose();
        }

        private async UniTaskVoid ScheduleAnimation(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await _animation.PlayThrowableAsync(_animation.clip.name, token);
                    await UniTask.Delay(TimeSpan.FromSeconds(_animationPlayPeriod), cancellationToken: token);
                }
            }
            catch (OperationCanceledException) { }
        }
    }
}