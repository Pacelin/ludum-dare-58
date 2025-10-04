using UnityEngine;

namespace Scripts.Game.Flowers
{
    public class Flower : DrawableObject
    {
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