using System;
using DG.Tweening;
using JetBrains.Annotations;
using R3;

namespace Scripts.Core.Lifetime.Impl
{
    [PublicAPI]
    public static class DOTweenExtensions
    {
        public static Tween WithRuntimePause(this Tween tween, Action onKillCallback = null)
        {
            if (!tween.active)
                return tween;
            var subscription = ApplicationState.IsPaused.Subscribe(isPaused =>
            {
                if (isPaused)
                    tween.Pause();
                else
                    tween.Play();
            });
            tween.OnKill(() =>
            {
                subscription.Dispose();
                onKillCallback?.Invoke();
            });
            return tween;
        }
    }
}