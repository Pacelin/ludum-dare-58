using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scripts.Core.Lifetime.Impl
{
    sealed class DelayWithPausePromise : IUniTaskSource, IPlayerLoopItem, ITaskPoolNode<DelayWithPausePromise>
    {
        static TaskPool<DelayWithPausePromise> pool;
        DelayWithPausePromise nextNode;
        public ref DelayWithPausePromise NextNode => ref nextNode;

        static DelayWithPausePromise()
        {
            TaskPool.RegisterSizeGetter(typeof(DelayWithPausePromise), () => pool.Size);
        }

        int initialFrame;
        float delayTimeSpan;
        float elapsed;
        CancellationToken cancellationToken;
        CancellationTokenRegistration cancellationTokenRegistration;
        bool cancelImmediately;

        UniTaskCompletionSourceCore<object> core;

        DelayWithPausePromise()
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
                result = new DelayWithPausePromise();
            }

            result.elapsed = 0.0f;
            result.delayTimeSpan = (float)delayTimeSpan.TotalSeconds;
            result.cancellationToken = cancellationToken;
            result.initialFrame = PlayerLoopHelper.IsMainThread ? Time.frameCount : -1;
            result.cancelImmediately = cancelImmediately;

            if (cancelImmediately && cancellationToken.CanBeCanceled)
            {
                result.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(state =>
                {
                    var promise = (DelayWithPausePromise)state;
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
            if (cancellationToken.IsCancellationRequested)
            {
                core.TrySetCanceled(cancellationToken);
                return false;
            }
            
            if (elapsed == 0.0f)
            {
                if (initialFrame == Time.frameCount)
                {
                    return true;
                }
            }
            
            if (ApplicationState.IsPaused.CurrentValue)
                return true;

            elapsed += Time.deltaTime;
            if (elapsed >= delayTimeSpan)
            {
                core.TrySetResult(null);
                return false;
            }

            return true;
        }

        bool TryReturn()
        {
            TaskTracker.RemoveTracking(this);
            core.Reset();
            delayTimeSpan = default;
            elapsed = default;
            cancellationToken = default;
            cancellationTokenRegistration.Dispose();
            cancelImmediately = default;
            return pool.TryPush(this);
        }
    }
}