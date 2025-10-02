using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace Scripts.Core.Lifetime.Impl
{
    [PublicAPI]
    public static class UniTaskUtils
    {
        public static UniTask DelayWithPause(int millisecondsDelay, bool ignoreTimeScale = false, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
        {
            var delayTimeSpan = TimeSpan.FromMilliseconds(millisecondsDelay);
            return DelayWithPause(delayTimeSpan, ignoreTimeScale, delayTiming, cancellationToken, cancelImmediately);
        }

        public static UniTask DelayWithPause(float secondsDelay, bool ignoreTimeScale = false, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
        {
            var delayTimeSpan = TimeSpan.FromSeconds(secondsDelay);
            return DelayWithPause(delayTimeSpan, ignoreTimeScale, delayTiming, cancellationToken, cancelImmediately);
        }

        public static UniTask DelayWithPause(TimeSpan delayTimeSpan, bool ignoreTimeScale = false, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
        {
            var delayType = ignoreTimeScale ? DelayType.UnscaledDeltaTime : DelayType.DeltaTime;
            return DelayWithPause(delayTimeSpan, delayType, delayTiming, cancellationToken, cancelImmediately);
        }

        public static UniTask DelayWithPause(int millisecondsDelay, DelayType delayType, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
        {
            var delayTimeSpan = TimeSpan.FromMilliseconds(millisecondsDelay);
            return DelayWithPause(delayTimeSpan, delayType, delayTiming, cancellationToken, cancelImmediately);
        }
        
        public static UniTask DelayWithPause(float secondsDelay, DelayType delayType, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
        {
            var delayTimeSpan = TimeSpan.FromMilliseconds(secondsDelay);
            return DelayWithPause(delayTimeSpan, delayType, delayTiming, cancellationToken, cancelImmediately);
        }
        
        public static UniTask DelayWithPause(TimeSpan delayTimeSpan, DelayType delayType, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
        {
            if (delayTimeSpan < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("Delay does not allow minus delayTimeSpan. delayTimeSpan:" + delayTimeSpan);
            }

#if UNITY_EDITOR
            // force use Realtime.
            if (PlayerLoopHelper.IsMainThread && !UnityEditor.EditorApplication.isPlaying)
            {
                delayType = DelayType.Realtime;
            }
#endif

            switch (delayType)
            {
                case DelayType.UnscaledDeltaTime:
                {
                    return new UniTask(DelayIgnoreTimeScaleWithPausePromise.Create(delayTimeSpan, delayTiming, cancellationToken, cancelImmediately, out var token), token);
                }
                case DelayType.Realtime:
                {
                    return new UniTask(DelayRealtimeWithPausePromise.Create(delayTimeSpan, delayTiming, cancellationToken, cancelImmediately, out var token), token);
                }
                case DelayType.DeltaTime:
                default:
                {
                    return new UniTask(DelayWithPausePromise.Create(delayTimeSpan, delayTiming, cancellationToken, cancelImmediately, out var token), token);
                }
            }
        }
    }
}