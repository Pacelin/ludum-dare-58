using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.MainMenu
{
    public class SmoothSpawn : MonoBehaviour, ISpawnCallback, IPointerClickHandler
    {
        [SerializeField] private float _lifetime = 60;

        private bool _destroyed;
        private RandomSpawn _spawn;
        
        private void OnEnable() => transform.DOScale(Random.Range(0.4f, 1.5f), 0.5f).From(0);
        private void OnDisable() => transform.DOKill();

        private void Update()
        {
            if (_destroyed) return;
            _lifetime -= Time.deltaTime;
            if (_lifetime <= 0)
            {
                _destroyed = true;
                transform.DOScale(0, 0.5f)
                    .OnComplete(() => Destroy(gameObject));
            }
        }

        public void OnSpawn(RandomSpawn spawn)
        {
            _spawn = spawn;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_destroyed) 
                return;
            _destroyed = true;
            DOTween.Sequence(transform)
                .Append(transform.DOScale(transform.localScale.x + 0.2f, 0.05f))
                .Append(transform.DOScale(0f, 0.05f))
                .OnComplete(() => Destroy(gameObject));
            _spawn.Spawn();
        }
    }
}