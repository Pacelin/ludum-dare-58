using UnityEngine;

namespace Scripts.Game.Flowers
{
    public class FlowersHotbarElement : HotbarElement
    {
        [SerializeField] private Flower _flower;

        public override void UpdateSelection(DrawCollider drawCollider, bool selected)
        {
            base.UpdateSelection(drawCollider, selected);
            drawCollider.SetDrawObject(_flower);
        }
    }
}