using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Game.Flowers
{
    public class DrawCollider : MonoBehaviour
    {
        public DrawField DrawField => _drawField;
        
        [SerializeField] private DrawField _drawField;
        [SerializeField] private DrawableObject _selectedDrawObject;
        [SerializeField] private float _drawCooldown = 0.1f;
        [SerializeField] private LayerMask _drawLayerMask;
        [SerializeField] private float _drawRadius;

        private Camera _camera;
        private float _lastDrawSeconds;
        
        public void SetDrawObject(DrawableObject drawableObject) =>
            _selectedDrawObject = drawableObject;
        
        private void FixedUpdate()
        {
            var elapsedSeconds = Time.time - _lastDrawSeconds;
            if (elapsedSeconds < _drawCooldown)
                return;
            if (!Input.GetMouseButton((int) MouseButton.LeftMouse))
                return;
            if (!_camera)
                _camera = Camera.main;
            
            var mousePosition = Input.mousePosition;
            mousePosition.z = 0;
            var point = (Vector2) _camera!.ScreenToWorldPoint(mousePosition);
            var overlaps = Physics2D.OverlapPointAll(point, _drawLayerMask);
            if (overlaps.All(o => o.TryGetComponent<DrawCollider>(out _)))
                Draw(point);
        }

        private void Draw(Vector2 point)
        {
            if (_selectedDrawObject)
                _drawField.Draw(point, _drawRadius, _selectedDrawObject);
            else
                _drawField.Erase(point, _drawRadius);
        }
    }
}