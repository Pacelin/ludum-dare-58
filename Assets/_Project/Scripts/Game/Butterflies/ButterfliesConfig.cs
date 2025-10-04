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
        
        [SerializeField] private Vector2 _spawnCooldownRange;
        [SerializeField] private float _flowersFindRadius;
        [SerializeField] private float _doubleFlowerChance;
        [SerializeField] private float[] _specialWeights;
        [Space]
        [SerializeField] private ButterflyView[] _butterflies;

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
            
            Debug.LogError("No butterfly found for flower ids: " + string.Join(", ", flowerIds));
            return null;
        }
        public int GetButterflyID(ButterflyView prefab) => System.Array.IndexOf(_butterflies, prefab);
    }
}