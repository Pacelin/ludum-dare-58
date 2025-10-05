using UnityEngine;
using UnityEngine.Localization;

namespace Scripts.Game.Butterflies
{
    [System.Serializable]
    public struct ButterflySizeConfig
    {
        public LocalizedString Prefix => _prefix;
        public Vector2 AverageSizeMultiplierRange => _averageSizeMultiplierRange;
        public float CostMutiplier => _costMutiplier;
        
        [SerializeField] private LocalizedString _prefix;
        [SerializeField] private Vector2 _averageSizeMultiplierRange;
        [SerializeField] private float _costMutiplier;
    }
}