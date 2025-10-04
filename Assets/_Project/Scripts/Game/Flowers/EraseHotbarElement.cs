namespace Scripts.Game.Flowers
{
    public class EraseHotbarElement : HotbarElement
    {
        public override void UpdateSelection(DrawCollider drawCollider, bool selected)
        {
            base.UpdateSelection(drawCollider, selected);
            drawCollider.SetDrawObject(null);
        }
    }
}