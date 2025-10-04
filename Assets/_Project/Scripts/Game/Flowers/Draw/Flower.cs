using UnityEngine;

namespace Scripts.Game.Flowers
{
    public class Flower : DrawableObject
    {
        [SerializeField] private int _id;

        public virtual int GetId(GameTime time) => _id;
        
        public override void OnDraw(Vector2 point)
        {
            gameObject.SetActive(true);
            gameObject.transform.position = point;
        }

        public override void OnErase()
        {
            gameObject.SetActive(false);
        }
    }
}