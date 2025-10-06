using R3;
using UnityEngine;

namespace Scripts.Core.Lifetime
{
    internal class ApplicationMonoProvider : MonoBehaviour
    {
        public ReadOnlyReactiveProperty<bool> IsPaused => _hasFocus
            .CombineLatest(_isPaused, _pauseGameplay, 
            (p1, p2, p3) => !p1 || p2 || p3)
            .ToReadOnlyReactiveProperty();
        public ReadOnlyReactiveProperty<bool> IsPausedByUser => _isPaused;

        private readonly ReactiveProperty<bool> _hasFocus = new(true);
        private readonly ReactiveProperty<bool> _isPaused = new(false);
        private readonly ReactiveProperty<bool> _pauseGameplay = new(false);

        private void Awake() => _hasFocus.Value = Application.isFocused;
        public void SetPause(bool pause) => _isPaused.Value = pause;
        public void SetPauseGameplay(bool pause) => _pauseGameplay.Value = pause;
        
        private void OnApplicationFocus(bool hasFocus) => _hasFocus.Value = hasFocus;
    }
}