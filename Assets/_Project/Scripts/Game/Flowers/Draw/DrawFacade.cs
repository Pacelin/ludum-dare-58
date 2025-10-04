using UnityEngine;

namespace Scripts.Game.Flowers
{
    public class DrawFacade
    {
        public DrawCollider DrawCollider => _drawCollider;
        public DrawField DrawField => _drawCollider.DrawField;
        
        private readonly DrawCollider _drawCollider;
        
        public DrawFacade(DrawCollider drawCollider) =>
            _drawCollider = drawCollider;
        
        public void SetCanDraw(bool canDraw) => _drawCollider.DrawField.SetCanDraw(canDraw);
        public void SetCanErase(bool canErase) => _drawCollider.DrawField.SetCanErase(canErase);
        public void SetDrawObject(DrawableObject drawableObject) => _drawCollider.SetDrawObject(drawableObject);
        public T[] GetDrawnObjects<T>(Vector2 point, float radius) where T : DrawableObject 
            => _drawCollider.DrawField.GetDrawnObjects<T>(point, radius);
        public T GetRandomDrawnObject<T>() where T : DrawableObject => _drawCollider.DrawField.GetRandomDrawnObject<T>();
    }
}