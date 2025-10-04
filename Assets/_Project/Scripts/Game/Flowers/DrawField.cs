using System.Collections.Generic;
using Scripts.Game.Flowers.Spatial;
using UnityEngine;

namespace Scripts.Game.Flowers
{
    public class DrawField : MonoBehaviour
    {
        [SerializeField] private SpatialGridContainer _gridContainer;
        
        private readonly Dictionary<Vector2, DrawableObject> _drawnObjects = new();
        private readonly Dictionary<string, Queue<DrawableObject>> _drawObjectsPool = new();

        public T[] GetDrawnObjects<T>(Vector2 point, float radius)
            where T : DrawableObject
        {
            var result = new List<T>();
            var pointsInCircle = _gridContainer.SpatialGrid.GetPointsInCircle(point, radius);
            foreach (var p in pointsInCircle)
                if (_drawnObjects.TryGetValue(p, out var obj) && obj is T t)
                    result.Add(t);
            
            return result.ToArray();
        }
        
        public void Draw(Vector2 position, float drawRadius, DrawableObject prefab)
        {
            var drawPoints = _gridContainer.SpatialGrid.GetPointsInCircle(position, drawRadius);
            foreach (var point in drawPoints)
                DrawPoint(point, prefab);
        }

        public void Erase(Vector2 position, float drawRadius)
        {
            var erasePoints = _gridContainer.SpatialGrid.GetPointsInCircle(position, drawRadius);
            foreach (var point in erasePoints)
                ErasePoint(point);
        }

        private void ErasePoint(Vector2 point)
        {
            if (_drawnObjects.TryGetValue(point, out var obj))
            {
                obj.OnErase();
                _drawObjectsPool[obj.DrawId].Enqueue(obj);
                _drawnObjects.Remove(point);
            }
        }
        
        private void DrawPoint(Vector2 point, DrawableObject prefab)
        {
            if (_drawnObjects.TryGetValue(point, out var obj))
            {
                if (obj.DrawId == prefab.DrawId) 
                    return;
                obj.OnErase();
                _drawObjectsPool[obj.DrawId].Enqueue(obj);
            }
            
            var drawObject = GetDrawObject(prefab);
            drawObject.OnDraw(point);
            _drawnObjects.Add(point, drawObject);
        }

        private DrawableObject GetDrawObject(DrawableObject prefab)
        {
            if (!_drawObjectsPool.ContainsKey(prefab.DrawId))
                _drawObjectsPool.Add(prefab.DrawId, new Queue<DrawableObject>());
            if (_drawObjectsPool[prefab.DrawId].TryDequeue(out var obj))
                return obj;
            return Instantiate(prefab);
        }
    }
}