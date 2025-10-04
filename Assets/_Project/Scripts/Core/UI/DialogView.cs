using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Scripts.Audio;
using UnityEngine;
using VContainer;

namespace Scripts.Core.UI
{
    public class DialogView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _content;
        [SerializeField] private Vector2 _contentClosedPosition = new Vector2(0, -1400);
        [SerializeField] private Vector2 _contentOpenPosition = new Vector2(0, 0);
        [SerializeField] private float _openCloseDuration = 0.4f;

        [Inject]
        private void Construct()
        {
            _content.anchoredPosition = _contentClosedPosition;
            _canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }
        
        protected virtual void OnDisable() => DOTween.Kill(this);

        public UniTask Open(CancellationToken cancellationToken)
        {
            AudioSystem.UI_PopupAppear.PlayOneShot();
            return DOTween.Sequence(this)
                .Append(_canvasGroup.DOFade(1, _openCloseDuration))
                .Join(_content.DOAnchorPos(_contentOpenPosition, _openCloseDuration))
                .OnStart(() => gameObject.SetActive(true))
                .ToUniTask(cancellationToken: cancellationToken, tweenCancelBehaviour: TweenCancelBehaviour.KillAndCancelAwait);
        }

        public UniTask Close(CancellationToken cancellationToken)
        {
            return DOTween.Sequence(this)
                .Append(_canvasGroup.DOFade(0, _openCloseDuration))
                .Join(_content.DOAnchorPos(_contentClosedPosition, _openCloseDuration))
                .OnComplete(() => gameObject.SetActive(false))
                .ToUniTask(cancellationToken: cancellationToken, tweenCancelBehaviour: TweenCancelBehaviour.KillAndCancelAwait);
        }
    }
}