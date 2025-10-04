using UnityEngine;

namespace Scripts.Game.Flowers
{
    public class FlowersHotbarElement : HotbarElement
    {
        public Flower Flower => _flower;
        
        [SerializeField] private Flower _flower;

        public override void Visit(HotbarController hotbarController) => hotbarController.Accept(this);
    }
}