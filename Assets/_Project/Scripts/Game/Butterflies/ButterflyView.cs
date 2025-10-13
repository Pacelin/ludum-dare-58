using System;
using DG.Tweening;
using Scripts.Core.InspectorCustomization;
using Scripts.Game.Butterflies.ButterfliesJournal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Scripts.Game.Butterflies
{
    public class ButterflyView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public ButterflyConfig Config => _config;
        
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Header("Spawn")]
        [SerializeField] private float _startHeight;
        [SerializeField] private float _endHeight;
        [SerializeField] private float _spawnDuration;
        [Header("Catch")]
        [SerializeField] private float _catchDuration;
        [SerializeField] private CircleCollider2D _collider;
        [SerializeField] private float _minColliderRadius;
        [DrawInBox(ShowLabel = true)]
        [SerializeField] private ButterflyConfig _config;
        
        private ButterfliesConfig _butterfliesConfig;
        private int _id;
        private ButterfliesCatcher _catcher;
        private float _size;
        private float _lifetime;
        private bool _destroyed;
        private bool _isHover;
        
        private static int HoverButterflies = 0;
        
        public ButterfliesJournalEntry CreateJournalEntry() => new ButterfliesJournalEntry()
        {
            Id = _id,
            Size = _size
        };
        
        public void Initialize(ButterfliesConfig butterfliesConfig,
            int id, ButterfliesCatcher catcher, Vector2 position, float size)
        {
            _butterfliesConfig = butterfliesConfig;
            _id = id;
            _catcher = catcher;
            _size = size;
            var scale = butterfliesConfig.GetScaleForWorld(Config, size);
            var startPosition = new Vector3(position.x, position.y + _startHeight, 0);
            var endPosition = new Vector3(position.x, position.y + _endHeight, 0);
            transform.DOMove(endPosition, _spawnDuration).From(startPosition);
            transform.DOScale(scale, _spawnDuration).From(0);
            if (_collider.radius * scale < _minColliderRadius)
                _collider.radius = _minColliderRadius / scale;
            _lifetime = 60;
            _destroyed = false;
        }

        private void OnDisable()
        {
            transform.DOKill();
            OnPointerExit(null);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_destroyed) return;
            _destroyed = true;
            transform.DOScale(0, _catchDuration)
                .OnComplete(() => Destroy(gameObject));
            _catcher.Catch(this);
            OnPointerExit(null);
        }

        private void Update()
        {
            _lifetime -= Time.deltaTime;
            if (_lifetime <= 0)
            {
                _destroyed = true;
                transform.DOScale(0, _catchDuration)
                    .OnComplete(() => Destroy(gameObject));
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHover = true;
            HoverButterflies++;
            UpdateCursor();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isHover) 
                return;
            _isHover = false;
            HoverButterflies--;
            UpdateCursor();
        }

        private void UpdateCursor()
        {
            if (HoverButterflies > 0)
                Cursor.SetCursor(_butterfliesConfig.CursorCatch, Vector2.zero, CursorMode.Auto);
            else
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}