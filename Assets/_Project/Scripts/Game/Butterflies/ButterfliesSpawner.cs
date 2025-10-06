using Scripts.Core.Lifetime;
using Scripts.Game.Flowers;
using UnityEngine;
using VContainer.Unity;

namespace Scripts.Game.Butterflies
{
    public class ButterfliesSpawner : ITickable
    {
        private readonly ButterfliesConfig _config;
        private readonly DrawFacade _drawFacade;
        private readonly ButterfliesCatcher _catcher;
        private readonly GameTime _time;
        
        private float _countdown;
        
        public ButterfliesSpawner(ButterfliesConfig config, DrawFacade drawFacade, 
            ButterfliesCatcher catcher, GameTime time)
        {
            _config = config;
            _drawFacade = drawFacade;
            _catcher = catcher;
            _time = time;
            ResetCountdown();
        }
        
        public void Tick()
        {
            if (ApplicationState.IsPaused.CurrentValue)
                return;
            _countdown -= Time.deltaTime;
            if (_countdown <= 0)
            {
                TrySpawn();
                ResetCountdown();
            }
        }

        private void TrySpawn()
        {
            var randomFlower = ButterfliesUtils.GetRandomFlower(_drawFacade);
            if (randomFlower == null)
                return;
            var randomButterfly = ButterfliesUtils.CalculateButterfly(_config, randomFlower, _drawFacade, _time, out var flowersCount);
            if (randomButterfly == null)
                return;
            var size = ButterfliesUtils.CalculateSize(_config, randomButterfly.Config, flowersCount);

            var id = _config.GetButterflyID(randomButterfly);
            var butterfly = Object.Instantiate(randomButterfly);
            butterfly.Initialize(_config, id, _catcher, randomFlower.transform.position, size);
        }
        
        private void ResetCountdown()
        {
            _countdown = Random.Range(_config.SpawnCooldownRange.x, _config.SpawnCooldownRange.y);
        }
    }
}