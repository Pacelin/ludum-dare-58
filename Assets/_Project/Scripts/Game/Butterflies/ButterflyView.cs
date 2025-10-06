using System;
using DG.Tweening;
using Scripts.Core.InspectorCustomization;
using Scripts.Game.Butterflies.ButterfliesJournal;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Game.Butterflies
{
    public class ButterflyView : MonoBehaviour, IPointerClickHandler
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
        
        private int _id;
        private ButterfliesCatcher _catcher;
        private float _size;
        private float _lifetime;
        private bool _destroyed;

        public ButterfliesJournalEntry CreateJournalEntry() => new ButterfliesJournalEntry()
        {
            Id = _id,
            Size = _size
        };
        
        public void Initialize(ButterfliesConfig butterfliesConfig,
            int id, ButterfliesCatcher catcher, Vector2 position, float size)
        {
            _id = id;
            _catcher = catcher;
            _size = size;
            var scale = butterfliesConfig.GetScaleForWorld(size);
            Debug.Log($"Butterfly {id} spawned with size {size:F}");
            var startPosition = new Vector3(position.x, position.y + _startHeight, 0);
            var endPosition = new Vector3(position.x, position.y + _endHeight, 0);
            transform.DOMove(endPosition, _spawnDuration).From(startPosition);
            transform.DOScale(scale, _spawnDuration).From(0);
            _collider.radius = Mathf.Max(_minColliderRadius / scale, _collider.radius * scale);
            _lifetime = 60;
            _destroyed = false;
        }

        private void OnDisable()
        {
            transform.DOKill();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_destroyed) return;
            _destroyed = true;
            transform.DOScale(0, _catchDuration)
                .OnComplete(() => Destroy(gameObject));
            _catcher.Catch(this);
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
    }
}