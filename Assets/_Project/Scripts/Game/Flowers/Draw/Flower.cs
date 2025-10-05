using DG.Tweening;
using UnityEngine;

namespace Scripts.Game.Flowers
{
    public class Flower : DrawableObject
    {
        public int UnlockPrice => _unlockPrice;
        
        [SerializeField] private int _unlockPrice;
        [SerializeField] private int _id;

        private void OnDisable()
        {
            transform.DOKill();
        }

        public virtual int GetId(GameTime time) => _id;
        
        public override void OnDraw(Vector2 point)
        {
            transform.DOKill();
            gameObject.transform.position = point;
            DOTween.Sequence(transform)
                .OnStart(() => gameObject.SetActive(true))
                .Append(transform.DOScale(1.2f, 0.075f).From(0).SetEase(Ease.Linear))
                .Append(transform.DOScale(1, 0.075f).SetEase(Ease.Linear))
                .SetEase(Ease.OutQuad);
        }

        public override void OnErase()
        {
            transform.DOKill();
            transform.DOScale(0, 0.1f)
                .OnComplete(() => gameObject.SetActive(false))
                .SetEase(Ease.InQuad);
        }
    }
}