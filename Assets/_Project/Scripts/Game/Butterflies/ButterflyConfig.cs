using UnityEngine;
using UnityEngine.Localization;

namespace Scripts.Game.Butterflies
{
    [System.Serializable]
    public class ButterflyConfig
    {
        public int SpecialIndex => _specialIndex;
        public int[] RequiredFlowers => _requiredFlowers;

        public float AverageSize => _averageSize;
        public int AverageCost => _averageCost;
        
        [Header("Info")] 
        [SerializeField] private int _specialIndex;
        [SerializeField] private int[] _requiredFlowers;
        [SerializeField] private LocalizedString _name;
        [Header("Calculation")]
        [SerializeField] private float _averageSize;
        [SerializeField] private int _averageCost;
    }
}