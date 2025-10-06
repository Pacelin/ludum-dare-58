using UnityEngine;

namespace Scripts.Game.Flowers
{
    public class SpecialFlower : Flower
    {
        [SerializeField] private int _nightId;

        public override int GetId(GameTime time)
        {
            if (time.IsNight)
                return _nightId;
            return base.GetId(time);
        }
    }
}