using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

namespace Scripts.Core.Lifetime.Impl
{
    sealed class DelayRealtimeWithPausePromise : IUniTaskSource, IPlayerLoopItem, ITaskPoolNode<DelayRealtimeWithPausePromise>
    {
        static TaskPool<DelayRealtimeWithPausePromise> pool;
        DelayRealtimeWithPausePromise nextNode;
        public ref DelayRealtimeWithPausePromise NextNode => ref nextNode;

        static DelayRealtimeWithPausePromise()
        {
            TaskPool.RegisterSizeGetter(typeof(DelayRealtimeWithPausePromise), () => pool.Size);
        }

        long delayTimeSpanTicks;
        long pauseTimeSpanTicks;
        ValueStopwatch stopwatch;
        CancellationToken cancellationToken;
        CancellationTokenRegistration cancellationTokenRegistration;
        bool cancelImmediately;

        UniTaskCompletionSourceCore<AsyncUnit> core;

        DelayRealtimeWithPausePromise()
        {
        }

        public static IUniTaskSource Create(TimeSpan delayTimeSpan, PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately, out short token)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
            }

            if (!pool.TryPop(out var result))
            {
                result = new DelayRealtimeWithPausePromise();
            }

            result.stopwatch = ValueStopwatch.StartNew();
            result.pauseTimeSpanTicks = 0;
            result.delayTimeSpanTicks = delayTimeSpan.Ticks;
            result.cancellationToken = cancellationToken;
            result.cancelImmediately = cancelImmediately;

            if (cancelImmediately && cancellationToken.CanBeCanceled)
            {
                result.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(state =>
                {
                    var promise = (DelayRealtimeWithPausePromise)state;
                    promise.core.TrySetCanceled(promise.cancellationToken);
                }, result);
            }

            TaskTracker.TrackActiveTask(result, 3);

            PlayerLoopHelper.AddAction(timing, result);

            token = result.core.Version;
            return result;
        }

        public void GetResult(short token)
        {
            try
            {
                core.GetResult(token);
            }
            finally
            {
                if (!(cancelImmediately && cancellationToken.IsCancellationRequested))
                {
                    TryReturn();
                }
                else
                {
                    TaskTracker.RemoveTracking(this);
                }
            }
        }

        public UniTaskStatus GetStatus(short token)
        {
            return core.GetStatus(token);
        }

        public UniTaskStatus UnsafeGetStatus()
        {
            return core.UnsafeGetStatus();
        }

        public void OnCompleted(Action<object> continuation, object state, short token)
        {
            core.OnCompleted(continuation, state, token);
        }

        public bool MoveNext()
        {
            Profiler.BeginSample("DelayRealtimeWithPausePromise.MoveNext");
            if (cancellationToken.IsCancellationRequested)
            {
                core.TrySetCanceled(cancellationToken);
                Profiler.EndSample();
                return false;
            }

            if (ApplicationState.IsPaused.CurrentValue)
            {
                pauseTimeSpanTicks += TimeSpan.FromSeconds(Time.unscaledDeltaTime).Ticks;
                Profiler.EndSample();
                return true;
            }

            if (stopwatch.IsInvalid)
            {
                core.TrySetResult(AsyncUnit.Default);
                Profiler.EndSample();
                return false;
            }

            if (stopwatch.ElapsedTicks >= delayTimeSpanTicks - pauseTimeSpanTicks)
            {
                core.TrySetResult(AsyncUnit.Default);
                Profiler.EndSample();
                return false;
            }

            Profiler.EndSample();
            return true;
        }

        bool TryReturn()
        {
            TaskTracker.RemoveTracking(this);
            core.Reset();
            stopwatch = default;
            cancellationToken = default;
            cancellationTokenRegistration.Dispose();
            cancelImmediately = default;
            return pool.TryPush(this);
        }
    }
}