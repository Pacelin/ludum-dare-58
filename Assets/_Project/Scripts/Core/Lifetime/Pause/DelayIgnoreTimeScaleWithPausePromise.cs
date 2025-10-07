using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

namespace Scripts.Core.Lifetime.Impl
{
    sealed class DelayIgnoreTimeScaleWithPausePromise : IUniTaskSource, IPlayerLoopItem, ITaskPoolNode<DelayIgnoreTimeScaleWithPausePromise>
    {
        static TaskPool<DelayIgnoreTimeScaleWithPausePromise> pool;
        DelayIgnoreTimeScaleWithPausePromise nextNode;
        public ref DelayIgnoreTimeScaleWithPausePromise NextNode => ref nextNode;

        static DelayIgnoreTimeScaleWithPausePromise()
        {
            TaskPool.RegisterSizeGetter(typeof(DelayIgnoreTimeScaleWithPausePromise), () => pool.Size);
        }

        float delayFrameTimeSpan;
        float elapsed;
        int initialFrame;
        CancellationToken cancellationToken;
        CancellationTokenRegistration cancellationTokenRegistration;
        bool cancelImmediately;

        UniTaskCompletionSourceCore<object> core;

        DelayIgnoreTimeScaleWithPausePromise()
        {
        }

        public static IUniTaskSource Create(TimeSpan delayFrameTimeSpan, PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately, out short token)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
            }

            if (!pool.TryPop(out var result))
            {
                result = new DelayIgnoreTimeScaleWithPausePromise();
            }

            result.elapsed = 0.0f;
            result.delayFrameTimeSpan = (float)delayFrameTimeSpan.TotalSeconds;
            result.initialFrame = PlayerLoopHelper.IsMainThread ? Time.frameCount : -1;
            result.cancellationToken = cancellationToken;
            result.cancelImmediately = cancelImmediately;

            if (cancelImmediately && cancellationToken.CanBeCanceled)
            {
                result.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(state =>
                {
                    var promise = (DelayIgnoreTimeScaleWithPausePromise)state;
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
            Profiler.BeginSample("DelayIgnoreTimeScaleWithPausePromise.MoveNext");
            if (cancellationToken.IsCancellationRequested)
            {
                core.TrySetCanceled(cancellationToken);
                Profiler.EndSample();
                return false;
            }

            if (elapsed == 0.0f)
            {
                if (initialFrame == Time.frameCount)
                {
                    Profiler.EndSample();
                    return true;
                }
            }

            if (ApplicationState.IsPaused.CurrentValue)
            {
                Profiler.EndSample();
                return true;
            }

            elapsed += Time.unscaledDeltaTime;
            if (elapsed >= delayFrameTimeSpan)
            {
                core.TrySetResult(null);
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
            delayFrameTimeSpan = default;
            elapsed = default;
            cancellationToken = default;
            cancellationTokenRegistration.Dispose();
            cancelImmediately = default;
            return pool.TryPush(this);
        }
    }
}