using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Flowers.Spatial
{
    [System.Serializable]
    public class SpatialGrid : ISerializationCallbackReceiver
    {
        public IReadOnlyList<GridCellData> Grid => _serializedGrid;

        [SerializeField] private Vector2 _gridOrigin;
        [SerializeField] private float _cellSize;
        [SerializeField] private int _gridSizeX;
        [SerializeField] private int _gridSizeY;
    
        // Двумерный массив для хранения точек в каждой ячейке сетки
        private List<Vector2>[,] _grid;
    
        // Для сериализации: преобразуем _grid в линейный список
        [SerializeField] 
        private List<GridCellData> _serializedGrid = new List<GridCellData>();
    
        public Vector2 GridOrigin => _gridOrigin;
        public float CellSize => _cellSize;
        public int GridSizeX => _gridSizeX;
        public int GridSizeY => _gridSizeY;

        public SpatialGrid(Vector2 gridOrigin, float cellSize, int gridSizeX, int gridSizeY)
        {
            _gridOrigin = gridOrigin;
            _cellSize = cellSize;
            _gridSizeX = gridSizeX;
            _gridSizeY = gridSizeY;
        
            _grid = new List<Vector2>[gridSizeX, gridSizeY];
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    _grid[x, y] = new List<Vector2>();
                }
            }
        }

        // Преобразует мировую позицию в координаты ячейки сетки
        public (int, int) WorldToGridPosition(Vector2 worldPosition)
        {
            int x = Mathf.FloorToInt((worldPosition.x - _gridOrigin.x) / _cellSize);
            int y = Mathf.FloorToInt((worldPosition.y - _gridOrigin.y) / _cellSize);
            return (x, y);
        }

        // Добавляет точку в сетку
        public void AddPoint(Vector2 point)
        {
            (int gridX, int gridY) = WorldToGridPosition(point);
        
            if (gridX >= 0 && gridX < _gridSizeX && gridY >= 0 && gridY < _gridSizeY)
            {
                _grid[gridX, gridY].Add(point);
            }
        }

        // Получает точки в ячейке и соседних ячейках (радиус 1)
        public List<Vector2> GetPointsInArea(Vector2 worldPosition, int searchRadius = 1)
        {
            List<Vector2> points = new List<Vector2>();
            (int centerX, int centerY) = WorldToGridPosition(worldPosition);
        
            for (int x = centerX - searchRadius; x <= centerX + searchRadius; x++)
            {
                for (int y = centerY - searchRadius; y <= centerY + searchRadius; y++)
                {
                    if (x >= 0 && x < _gridSizeX && y >= 0 && y < _gridSizeY)
                    {
                        points.AddRange(_grid[x, y]);
                    }
                }
            }
            return points;
        }

        public List<Vector2> GetPointsInCircle(Vector2 worldPosition, float radius)
        {
            var pointsInArea = GetPointsInArea(worldPosition, Mathf.CeilToInt(radius));
            pointsInArea.RemoveAll(point => (point - worldPosition).magnitude > radius);
            return pointsInArea;
        }

        // Метод для преобразования точек из PoissonDiskSampling в SpatialGrid
        public static SpatialGrid FromPoissonDiscSampling(List<Vector2> poissonPoints, Vector2 gridOrigin, float cellSize, Vector2 gridSize)
        {
            int gridSizeX = Mathf.CeilToInt(gridSize.x / cellSize);
            int gridSizeY = Mathf.CeilToInt(gridSize.y / cellSize);
        
            SpatialGrid spatialGrid = new SpatialGrid(gridOrigin, cellSize, gridSizeX, gridSizeY);
        
            foreach (Vector2 point in poissonPoints)
            {
                spatialGrid.AddPoint(point);
            }
        
            return spatialGrid;
        }

        // Реализация методов интерфейса ISerializationCallbackReceiver для кастомной сериализации
        public void OnBeforeSerialize()
        {
            _serializedGrid.Clear();
            if (_grid == null) return;
        
            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    _serializedGrid.Add(new GridCellData()
                    {
                        cellX = x,
                        cellY = y,
                        points = new List<Vector2>(_grid[x, y])
                    });
                }
            }
        }

        public void OnAfterDeserialize()
        {
            if (_serializedGrid == null) return;
        
            _grid = new List<Vector2>[_gridSizeX, _gridSizeY];
            for (int i = 0; i < _gridSizeX; i++)
            {
                for (int j = 0; j < _gridSizeY; j++)
                {
                    _grid[i, j] = new List<Vector2>();
                }
            }
        
            foreach (GridCellData cellData in _serializedGrid)
            {
                if (cellData.cellX >= 0 && cellData.cellX < _gridSizeX && 
                    cellData.cellY >= 0 && cellData.cellY < _gridSizeY)
                {
                    _grid[cellData.cellX, cellData.cellY] = cellData.points;
                }
            }
        }
    }
}