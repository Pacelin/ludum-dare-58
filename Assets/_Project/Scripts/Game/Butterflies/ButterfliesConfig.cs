using System.Collections.Generic;
using System.Linq;
using Scripts.Core.UnityEditorHelpers;
using UnityEngine;

namespace Scripts.Game.Butterflies
{
    [CreateAsset("SO_ButterfliesConfig")]
    public class ButterfliesConfig : ScriptableObject
    {
        public Vector2 SpawnCooldownRange => _spawnCooldownRange;
        public float FlowersFindRadius => _flowersFindRadius;
        public float DoubleFlowerChance => _doubleFlowerChance;
        public float SizeMeanMultiplier => _sizeMeanMultiplier;
        public float SizeDeviationMultiplier => _sizeDeviationMultiplier;
        public float MinSizeMultiplier => _minSizeMultiplier;
        public float MaxSizeMultiplier => _maxSizeMultiplier;
        public Texture2D CursorCatch => _cursorCatch;
        public Texture2D DefaultCursor => _defaultCursor;
        
        [SerializeField] private Texture2D _cursorCatch;
        [SerializeField] private Texture2D _defaultCursor;
        [SerializeField] private Vector2 _spawnCooldownRange;
        [SerializeField] private float _flowersFindRadius;
        [SerializeField] private float _doubleFlowerChance;
        [SerializeField] private float[] _specialWeights;
        [Space]
        [SerializeField] private ButterflyView[] _butterflies;
        [Header("Size Calculation")]
        [SerializeField] private ButterflySizeConfig[] _sizeConfigs;
        [SerializeField] private float _minSizeMultiplier;
        [SerializeField] private float _maxSizeMultiplier;
        [SerializeField] private float _sizeMeanMultiplier;
        [SerializeField] private float _sizeDeviationMultiplier;
        [SerializeField] private Vector2 _flowersCountRange;
        [SerializeField] private Vector2 _flowersMeanMultiplierRange;
        [SerializeField] private Vector2 _butterflySizeRange;
        [SerializeField] private Vector2 _journalScaleRange;
        [SerializeField] private Vector2 _worldScaleRange;

        public float GetFlowerMeanMultiplier(int flowersCount)
        {
            var t = Mathf.InverseLerp(_flowersCountRange.x, _flowersCountRange.y, flowersCount);
            return Mathf.Lerp(_flowersMeanMultiplierRange.x, _flowersMeanMultiplierRange.y, t);
        }
        
        public ButterflySizeConfig GetButterflySizeConfig(ButterflyConfig butterfly, float size)
        {
            var averageSize = butterfly.AverageSize;
            var averageSizeMultiplier = size / averageSize;
            for (int i = 0; i < _sizeConfigs.Length; i++)
            {
                if (averageSizeMultiplier <= _sizeConfigs[i].AverageSizeMultiplierRange.x)
                    return _sizeConfigs[i];
                if (averageSizeMultiplier > _sizeConfigs[i].AverageSizeMultiplierRange.x &&
                    averageSizeMultiplier <= _sizeConfigs[i].AverageSizeMultiplierRange.y)
                    return _sizeConfigs[i];
                if (averageSizeMultiplier > _sizeConfigs[i].AverageSizeMultiplierRange.y &&
                    i == _sizeConfigs.Length - 1)
                    return _sizeConfigs[i];
            }
            return _sizeConfigs[0];
        }

        public float GetScaleForJournal(ButterflyConfig butterflyConfig, float size)
        {
            var sizeConfig = GetButterflySizeConfig(butterflyConfig, size);
            var t = Mathf.InverseLerp(_butterflySizeRange.x, _butterflySizeRange.y, 
                size * sizeConfig.ScaleInJournalMultiplier);
            return Mathf.Lerp(_journalScaleRange.x, _journalScaleRange.y, Mathf.Clamp01(t));
        }

        public float GetScaleForWorld(ButterflyConfig butterflyConfig, float size)
        {
            var sizeConfig = GetButterflySizeConfig(butterflyConfig, size);
            var t = Mathf.InverseLerp(_butterflySizeRange.x, _butterflySizeRange.y, 
                size * sizeConfig.ScaleInWorldMultiplier);
            return Mathf.Lerp(_worldScaleRange.x, _worldScaleRange.y, Mathf.Clamp01(t));
        }
        
        public ButterflySizeConfig GetButterflySizeConfig(int index) => _sizeConfigs[index];
        
        public ButterflyView GetRandomButterfly() => _butterflies[Random.Range(0, _butterflies.Length)];
        public ButterflyView GetButterflyById(int id) => _butterflies[id];
        
        public ButterflyView GetButterfly(params int[] flowerIds)
        {
            var successButterflies = new List<ButterflyView>();
            foreach (var butterfly in _butterflies)
            {
                var requiredFlowerIds = butterfly.Config.RequiredFlowers;
                if (requiredFlowerIds.Length != flowerIds.Length)
                    continue;
                if (requiredFlowerIds.All(flowerId => flowerIds.Contains(flowerId)))
                    successButterflies.Add(butterfly);
            }
            
            var weightsSum = successButterflies.Sum(b => _specialWeights[b.Config.SpecialIndex]);
            var r = Random.Range(0f, weightsSum);
            var accumulatedWeight = 0f;
            foreach (var butterfly in successButterflies)
            {
                accumulatedWeight += _specialWeights[butterfly.Config.SpecialIndex];
                if (r <= accumulatedWeight) return butterfly;
            }
            
            if (successButterflies.Count > 0)
                return successButterflies[0];
            
            return GetButterfly(flowerIds[0]);
        }
        public int GetButterflyID(ButterflyView prefab) => System.Array.IndexOf(_butterflies, prefab);
        public int GetButterfliesCount() => _butterflies.Length;
    }
}