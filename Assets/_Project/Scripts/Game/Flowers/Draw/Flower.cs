using System;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Game.Flowers
{
    public class Flower : DrawableObject
    {
        public int UnlockPrice => _unlockPrice;
        
        [SerializeField] private int _unlockPrice;
        [SerializeField] private int _id;

        private static Camera _camera;
        private float _targetScale;
        
        public virtual int GetId(GameTime time) => _id;
        
        public override void OnDraw(Vector2 point)
        {
            gameObject.transform.position = point;
            gameObject.SetActive(true);
        }

        public override void OnErase()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_camera)
                _camera = Camera.main;
            var mousePos = Input.mousePosition;
            mousePos.z = 0;
            var pos = _camera.ScreenToWorldPoint(mousePos);
            if (Vector2.Distance(pos, transform.position) < 0.5f)
                _targetScale = 0.5f;
            else 
                _targetScale = 1f;
            
            var curScale = transform.localScale;
            curScale.y = Mathf.Lerp(curScale.y, _targetScale, Time.deltaTime * 10);
            transform.localScale = curScale;
        }
    }
}