using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Flowers.Spatial
{
    [System.Serializable]
    public struct GridCellData
    {
        public int cellX;
        public int cellY;
        public List<Vector2> points;
    }
}