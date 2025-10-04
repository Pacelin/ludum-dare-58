using UnityEngine;

namespace Scripts.Game.Flowers
{
    public abstract class DrawableObject : MonoBehaviour
    {
        public string DrawId => _drawId;
        public int DrawPrice => _drawPrice;
        public int EraseReward => _eraseReward;
        
        [SerializeField] private string _drawId;
        [SerializeField] private int _drawPrice = 1;
        [SerializeField] private int _eraseReward = 0;
        
        public abstract void OnDraw(Vector2 point);
        public abstract void OnErase();
    }
}