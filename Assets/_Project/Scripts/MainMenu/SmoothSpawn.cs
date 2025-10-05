using DG.Tweening;
using UnityEngine;

namespace Scripts.MainMenu
{
    public class SmoothSpawn : MonoBehaviour
    {
        [SerializeField] private float _lifetime = 60;

        private bool _destroyed;
        
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
    }
}