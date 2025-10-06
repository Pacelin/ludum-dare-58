using System;
using UnityEngine;

namespace Scripts.Core.UI
{
    [ExecuteInEditMode]
    public class InverseScaleConstraint : MonoBehaviour
    {
        [SerializeField] private Transform _source;
        [SerializeField] private Transform _target;

        private void Update()
        {
#if UNITY_EDITOR
            if (!_source || !_target) 
                return;
#endif
            
            var curScale = _target.localScale.x;
            var targetScale = 1 / _source.localScale.x;
            if (Math.Abs(curScale - targetScale) > 0.001f)
                _target.localScale = new Vector3(targetScale, targetScale, targetScale);
        }
    }
}