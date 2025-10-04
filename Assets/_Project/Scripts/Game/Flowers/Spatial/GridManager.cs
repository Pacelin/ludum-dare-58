using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Flowers.Spatial
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private SpriteRenderer _targetSpriteRenderer;
        [SerializeField] private float _alphaThreshold = 0.1f;
        [SerializeField] private SpatialGridContainer _spatialGrid;
        
        [Header("Grid Settings")]
        [SerializeField] private Vector2 _gridOrigin = Vector2.zero;
        [SerializeField] private float _cellSize = 5f;
        
        private Texture2D _spriteTexture;
        private Color[] _spritePixels;
        private Vector2 _spriteTextureSize;
        
        [ContextMenu("Generate Poisson Grid From Sprite")]
        public void GeneratePoissonGridFromSprite()
        {
            if (_targetSpriteRenderer == null || _targetSpriteRenderer.sprite == null)
            {
                Debug.LogError("Target SpriteRenderer is not assigned or has no sprite!");
                return;
            }
            
            // Подготовка данных спрайта
            PrepareSpriteData();
            
            // Генерация точек с учетом прозрачности спрайта
            List<Vector2> points = GeneratePointsInSprite(_radius);
            
            // Преобразуем точки в SpatialGrid
            Vector2 spriteSize = _targetSpriteRenderer.bounds.size;
            _spatialGrid.SetSpatialGrid(SpatialGrid.FromPoissonDiscSampling(
                points, _gridOrigin, _cellSize, spriteSize));
            
            Debug.Log($"SpatialGrid created with {points.Count} points from sprite");
        }
        
        private void PrepareSpriteData()
        {
            Sprite sprite = _targetSpriteRenderer.sprite;
            _spriteTexture = sprite.texture;
            
            // Получаем пиксели спрайта
            Rect textureRect = sprite.textureRect;
            _spritePixels = _spriteTexture.GetPixels(
                (int)textureRect.x, 
                (int)textureRect.y, 
                (int)textureRect.width, 
                (int)textureRect.height);
                
            _spriteTextureSize = new Vector2(textureRect.width, textureRect.height);
        }
        
        private List<Vector2> GeneratePointsInSprite(float radius)
        {
            List<Vector2> points = new List<Vector2>();
            List<Vector2> spawnPoints = new List<Vector2>();
            
            Bounds spriteBounds = _targetSpriteRenderer.bounds;
            Vector2 center = spriteBounds.center;
            Vector2 size = spriteBounds.size;
            
            // Начинаем с центра спрайта
            spawnPoints.Add(center);
            
            while (spawnPoints.Count > 0)
            {
                int spawnIndex = Random.Range(0, spawnPoints.Count);
                Vector2 spawnCentre = spawnPoints[spawnIndex];
                bool candidateAccepted = false;

                for (int i = 0; i < 30; i++)
                {
                    float angle = Random.value * Mathf.PI * 2;
                    Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                    Vector2 candidate = spawnCentre + dir * Random.Range(radius, 2 * radius);
                    
                    // Проверяем, что точка внутри спрайта и на непрозрачной области
                    if (IsPointInSpriteBounds(candidate) && IsPointOnOpaqueArea(candidate))
                    {
                        // Проверяем, что точка не слишком близко к другим точкам
                        if (IsValidPoint(candidate, radius, points, spriteBounds))
                        {
                            points.Add(candidate);
                            spawnPoints.Add(candidate);
                            candidateAccepted = true;
                            break;
                        }
                    }
                }
                
                if (!candidateAccepted)
                {
                    spawnPoints.RemoveAt(spawnIndex);
                }
            }
            
            return points;
        }
        
        private bool IsPointInSpriteBounds(Vector2 worldPoint)
        {
            Bounds bounds = _targetSpriteRenderer.bounds;
            return worldPoint.x >= bounds.min.x && worldPoint.x <= bounds.max.x &&
                   worldPoint.y >= bounds.min.y && worldPoint.y <= bounds.max.y;
        }
        
        private bool IsPointOnOpaqueArea(Vector2 worldPoint)
        {
            // Преобразуем мировые координаты в локальные координаты спрайта
            Vector3 localPoint = _targetSpriteRenderer.transform.InverseTransformPoint(worldPoint);
            
            // Преобразуем локальные координаты в UV-координаты
            Sprite sprite = _targetSpriteRenderer.sprite;
            Vector2 pivot = sprite.pivot;
            Vector2 spriteSize = sprite.rect.size;
            
            // Нормализованные координаты (0-1)
            float u = (localPoint.x + pivot.x / sprite.pixelsPerUnit) / (spriteSize.x / sprite.pixelsPerUnit);
            float v = (localPoint.y + pivot.y / sprite.pixelsPerUnit) / (spriteSize.y / sprite.pixelsPerUnit);
            
            if (u < 0 || u >= 1 || v < 0 || v >= 1)
                return false;
            
            // Преобразуем UV в координаты текстуры
            int x = Mathf.FloorToInt(u * _spriteTextureSize.x);
            int y = Mathf.FloorToInt(v * _spriteTextureSize.y);
            
            int index = y * (int)_spriteTextureSize.x + x;
            
            if (index < 0 || index >= _spritePixels.Length)
                return false;
            
            // Проверяем альфа-канал :cite[3]
            return _spritePixels[index].a > _alphaThreshold;
        }
        
        private bool IsValidPoint(Vector2 candidate, float radius, List<Vector2> points, Bounds bounds)
        {
            float cellSize = radius / Mathf.Sqrt(2);
            int gridSizeX = Mathf.CeilToInt(bounds.size.x / cellSize);
            int gridSizeY = Mathf.CeilToInt(bounds.size.y / cellSize);
            
            // Простая проверка расстояния до всех точек (можно оптимизировать с помощью сетки)
            foreach (Vector2 point in points)
            {
                if (Vector2.Distance(candidate, point) < radius)
                    return false;
            }
            return true;
        }
    }
}