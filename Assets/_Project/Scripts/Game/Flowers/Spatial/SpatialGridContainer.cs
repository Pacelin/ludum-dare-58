using UnityEngine;

namespace Scripts.Game.Flowers.Spatial
{
    public class SpatialGridContainer : MonoBehaviour
    {
        public SpatialGrid SpatialGrid => _spatialGrid;
        
        [SerializeField] private SpatialGrid _spatialGrid;
        
        public void SetSpatialGrid(SpatialGrid spatialGrid) => _spatialGrid = spatialGrid;
        
        private void OnDrawGizmosSelected()
        {
            if (_spatialGrid == null) return;

            var prevColor = Gizmos.color;
            foreach (var cell in _spatialGrid.Grid)
            {
                Gizmos.color = Random.ColorHSV();
                foreach (var point in cell.points)
                {
                    Gizmos.DrawSphere(point, 0.05f);
                }
            }
            Gizmos.color = prevColor;
        }
    }
}