using UnityEngine;

namespace Scripts.Game.Butterflies
{
    [System.Serializable]
    public class ButterflyConfig
    {
        public int SpecialIndex => _specialIndex;
        public Vector2Int CostRange => _costRange;
        public int[] RequiredFlowers => _requiredFlowers;
        public Vector2 ScaleRange => _scaleRange;
        public Vector2 ScaleCoefRandomRange => _scaleCoefRandomRange;
        public float SpeedPow => _speedPow;

        [SerializeField] private int _specialIndex;
        [SerializeField] private Vector2Int _costRange;
        [SerializeField] private int[] _requiredFlowers;
        [SerializeField] private Vector2 _scaleRange;
        [SerializeField] private Vector2 _scaleCoefRandomRange;
        [SerializeField] private float _speedPow;
    }
}