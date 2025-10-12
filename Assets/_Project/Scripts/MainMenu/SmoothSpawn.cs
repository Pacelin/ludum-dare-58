using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.MainMenu
{
    public class SmoothSpawn : MonoBehaviour, ISpawnCallback, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _lifetime = 60;
        [SerializeField] private Texture2D _cursor;

        private bool _destroyed;
        private RandomSpawn _spawn;
        private bool _isHover;
        
        private static int HoverButterflies = 0;
        
        private void OnEnable() => transform.DOScale(Random.Range(0.4f, 1.5f), 0.5f).From(0);

        private void OnDisable()
        {
            transform.DOKill();
            OnPointerExit(null);
        }

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
                .Append(transform.DOScale(transform.localScale.x + 0.3f, 0.1f))
                .Append(transform.DOScale(0f, 0.05f))
                .OnComplete(() => Destroy(gameObject));
            _spawn.Spawn();
            OnPointerExit(null);
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
                Cursor.SetCursor(_cursor, Vector2.zero, CursorMode.ForceSoftware);
            else
                Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}