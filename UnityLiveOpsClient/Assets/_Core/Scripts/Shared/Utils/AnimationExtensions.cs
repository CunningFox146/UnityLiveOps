using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Shared.Utils
{
    public static class AnimationExtensions
    {
        public static async UniTask PlayAsync(
            this Animation animation,
            string animationName,
            CancellationToken cancellationToken)
        {
            try
            {
                await animation.PlayThrowableAsync(animationName, cancellationToken);
            }
            catch (OperationCanceledException) { }
        }

        public static async UniTask PlayThrowableAsync(
            this Animation animation,
            string animationName,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            animation.Play(animationName);
            await YieldAnimationAsync(animation, animationName, cancellationToken);
        }

        private static async UniTask YieldAnimationAsync(Animation animation,
            string animationName,
            CancellationToken cancellationToken)
        {
            do
            {
                await UniTask.Yield(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }
            while (animation != null
                   && animation.IsPlaying(animationName)
                   && animation.gameObject != null
                   && animation.gameObject.activeInHierarchy);
        }
    }
}