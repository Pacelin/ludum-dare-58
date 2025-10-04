using UnityEngine;

namespace Scripts.Game.Flowers
{
    public abstract class DrawableObject : MonoBehaviour
    {
        public string DrawId => _drawId;
        
        [SerializeField] private string _drawId;
        
        public abstract void OnDraw(Vector2 point);
        public abstract void OnErase();
    }
}