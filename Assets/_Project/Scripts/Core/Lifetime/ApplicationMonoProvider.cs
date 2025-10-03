using R3;
using UnityEngine;

namespace Scripts.Core.Lifetime
{
    internal class ApplicationMonoProvider : MonoBehaviour
    {
        public ReadOnlyReactiveProperty<bool> IsPaused => _hasFocus.CombineLatest(_isPaused, 
            (p1, p2) => !p1 || p2).ToReadOnlyReactiveProperty();
        public ReadOnlyReactiveProperty<bool> IsPausedByUser => _isPaused;

        private readonly ReactiveProperty<bool> _hasFocus = new(true);
        private readonly ReactiveProperty<bool> _isPaused = new(false);

        private void Awake() => _hasFocus.Value = Application.isFocused;
        public void SetPause(bool pause) => _isPaused.Value = pause;
        
        private void OnApplicationFocus(bool hasFocus) => _hasFocus.Value = hasFocus;
    }
}