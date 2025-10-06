using _Game.Core.Scripts;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Environment
{
    [ExecuteAlways]
    public class DayCycleSystem : MonoBehaviour
    {
        [SerializeField] private Volume _volume;

        [SerializeField] private Texture[] _luts;
        [SerializeField] private AnimationCurve _dayCycleCurve;

        [SerializeField] private float _fullDayNightTime = 60f;

        [SerializeField] private bool _debugInterpolate;
        [SerializeField] private bool _useUnityTime;
        [SerializeField, Range(0f, 1f)] private float _debugTime;

        [SerializeField] private LUTBlender _blender;

        private float _normalizedTime;

        private void Update()
        {
            if (!Application.isPlaying && !(_debugInterpolate || _useUnityTime))
                return;

            if (_useUnityTime)
                SetCycleTime(Time.time);

            float normalizedTime = _debugInterpolate ? _debugTime : _normalizedTime;
            float value = _dayCycleCurve.Evaluate(normalizedTime) * (_luts.Length - 1);

            int currentStateIndex = Mathf.FloorToInt(value);
            int nextStateIndex = currentStateIndex + 1;

            Texture currentLut = _luts[currentStateIndex];
            Texture nextLut = nextStateIndex < _luts.Length ? _luts[nextStateIndex] : null;

            float currentStateTime = value - currentStateIndex;

            if (nextLut == null) return;

            _blender.CreateBlend(currentLut, 1f, nextLut, 1f, 1f);
            _blender.SetPostProcessing(currentStateTime);
        }

        public void SetCycleTime(float timeInSeconds)
        {
            _normalizedTime = timeInSeconds / _fullDayNightTime % 1f;
        }
    }
}