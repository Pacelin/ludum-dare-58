using UnityEngine;
using UnityEngine.Localization;

namespace Scripts.Game.Butterflies
{
    [System.Serializable]
    public struct ButterflySizeConfig
    {
        public LocalizedString SizeString => _sizeString;
        public LocalizedString SizeStringFemale => _sizeStringFemale;
        public Vector2 AverageSizeMultiplierRange => _averageSizeMultiplierRange;
        public float CostMutiplier => _costMutiplier;
        
        [SerializeField] private LocalizedString _sizeString;
        [SerializeField] private LocalizedString _sizeStringFemale;
        [SerializeField] private Vector2 _averageSizeMultiplierRange;
        [SerializeField] private float _costMutiplier;
    }
}