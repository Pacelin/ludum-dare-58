using R3;
using UnityEngine;
using VContainer.Unity;

namespace Scripts.Game
{
    public class GameTime : ITickable
    {
        public float SecondsPerCycle => _secondsPerCycle;
        public float SecondsPerDay => _secondsPerCycle / 2;
        public ReadOnlyReactiveProperty<float> Seconds => _seconds;
        public bool IsNight => _seconds.Value <= SecondsPerDay;
        public bool IsDay => _seconds.Value > SecondsPerDay;

        [System.Serializable] 
        public struct Config
        {
            public float SecondsPerCycle;
            public float InitialTime;
        }

        private readonly float _secondsPerCycle;
        private readonly ReactiveProperty<float> _seconds;

        public GameTime(Config config)
        {
            _secondsPerCycle = config.SecondsPerCycle;
            _seconds = new ReactiveProperty<float>(config.InitialTime);
        }

        public void Tick()
        {
            var newSeconds = _seconds.Value + Time.deltaTime;
            if (newSeconds >= _secondsPerCycle)
                newSeconds -= _secondsPerCycle;
            _seconds.Value = newSeconds;
        }
    }
}