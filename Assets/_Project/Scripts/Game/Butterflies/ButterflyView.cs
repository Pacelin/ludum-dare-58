using DG.Tweening;
using Scripts.Core.InspectorCustomization;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Game.Butterflies
{
    public class ButterflyView : MonoBehaviour, IPointerClickHandler
    {
        public int Id => _id;
        public float Size => _size;
        public ButterflyConfig Config => _config;
        
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
        
        public void Initialize(ButterfliesConfig butterfliesConfig,
            int id, ButterfliesCatcher catcher, Vector2 position, float size)
        {
            _id = id;
            _catcher = catcher;
            _size = size;
            var scale = butterfliesConfig.GetScale(size);
            Debug.Log($"Butterfly {id} spawned with size {size:F}");
            var startPosition = new Vector3(position.x, position.y + _startHeight, 0);
            var endPosition = new Vector3(position.x, position.y + _endHeight, 0);
            transform.DOMove(endPosition, _spawnDuration).From(startPosition);
            transform.DOScale(scale, _spawnDuration).From(0);
            _collider.radius = Mathf.Max(_minColliderRadius / scale, _collider.radius * scale);
        }

        private void OnDisable()
        {
            transform.DOKill();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            transform.DOScale(0, _catchDuration)
                .OnComplete(() => Destroy(gameObject));
            _catcher.Catch(this);
        }
    }
}