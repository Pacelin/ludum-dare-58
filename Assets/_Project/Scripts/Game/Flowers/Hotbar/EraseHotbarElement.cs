namespace Scripts.Game.Flowers
{
    public class EraseHotbarElement : HotbarElement
    {
        public override void Visit(HotbarController hotbarController) => hotbarController.Accept(this);
    }
}