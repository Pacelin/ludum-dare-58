using R3;
using UnityEngine;
using UnityEngine.Profiling;

namespace Scripts.Core.Lifetime
{
    internal class ApplicationMonoProvider : MonoBehaviour
    {
        private ReadOnlyReactiveProperty<bool> _isPausedParent;
        public ReadOnlyReactiveProperty<bool> IsPaused => _isPausedParent;
        public ReadOnlyReactiveProperty<bool> IsPausedByUser => _isPaused;
        public ReadOnlyReactiveProperty<bool> HasFocus => _hasFocus;

        private readonly ReactiveProperty<bool> _hasFocus = new(true);
        private readonly ReactiveProperty<bool> _isPaused = new(false);
        private readonly ReactiveProperty<bool> _pauseGameplay = new(false);

        private void Awake()
        {
            _isPausedParent = _hasFocus
                .CombineLatest(_isPaused, _pauseGameplay,
                    (p1, p2, p3) => !p1 || p2 || p3)
                .ToReadOnlyReactiveProperty();

            _hasFocus.Value = Application.isFocused;
        }

        public void SetPause(bool pause) => _isPaused.Value = pause;
        public void SetPauseGameplay(bool pause)
        {
            Profiler.BeginSample("ApplicationMonoProvider.SetPauseGameplay");
            _pauseGameplay.Value = pause;
            Profiler.EndSample();
        }

        private void OnApplicationFocus(bool hasFocus) => _hasFocus.Value = hasFocus;
    }
}