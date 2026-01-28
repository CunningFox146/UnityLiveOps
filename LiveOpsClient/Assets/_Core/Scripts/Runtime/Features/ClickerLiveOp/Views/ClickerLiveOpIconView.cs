using System;
using System.Threading;
using System.Threading.Tasks;
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

        private void Start()
        {
            var token = destroyCancellationToken;
            ScheduleAnimation(token).Forget();
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

        public override void Expire()
        {
            throw new System.NotImplementedException();
        }
    }
}