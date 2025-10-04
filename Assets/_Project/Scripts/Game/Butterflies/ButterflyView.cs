using DG.Tweening;
using Scripts.Core.InspectorCustomization;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Game.Butterflies
{
    public class ButterflyView : MonoBehaviour, IPointerClickHandler
    {
        public int Id => _id;
        public float Scale => _scale;
        public ButterflyConfig Config => _config;
        
        [Header("Spawn")]
        [SerializeField] private float _startHeight;
        [SerializeField] private float _endHeight;
        [SerializeField] private float _spawnDuration;
        [Header("Catch")]
        [SerializeField] private float _catchDuration;
        [DrawInBox(ShowLabel = true)]
        [SerializeField] private ButterflyConfig _config;
        
        private int _id;
        private ButterfliesCatcher _catcher;
        private float _scale;
        
        public void Initialize(int id, ButterfliesCatcher catcher, float scale, Vector2 position)
        {
            _id = id;
            _catcher = catcher;
            _scale = scale;
            var startPosition = new Vector3(position.x, position.y + _startHeight, 0);
            var endPosition = new Vector3(position.x, position.y + _endHeight, 0);
            transform.DOMove(endPosition, _spawnDuration).From(startPosition);
            transform.DOScale(scale, _spawnDuration).From(0);
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