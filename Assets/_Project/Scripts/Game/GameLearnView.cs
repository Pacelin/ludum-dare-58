using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Scripts.Core.Lifetime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game
{
    public class GameLearnView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _black;
        [Space]
        [SerializeField] private RectTransform _firstMask;
        [SerializeField] private RectTransform _secondMask;
        [SerializeField] private RectTransform _thirdMask;
        [SerializeField] private RectTransform _fourthMask;
        [Space] 
        [SerializeField] private TMP_Text _firstText;
        [SerializeField] private TMP_Text _secondText;
        [SerializeField] private TMP_Text _thirdText;
        [SerializeField] private TMP_Text _fourthText;
        [Space] 
        [SerializeField] private Button _firstContinueButton;
        [SerializeField] private Button _secondContinueButton;
        [SerializeField] private Button _thirdContinueButton;
        [SerializeField] private Button _fourthContinueButton;
        [Space]
        [SerializeField] private CanvasGroup _firstContinueButtonGroup;
        [SerializeField] private CanvasGroup _secondContinueButtonGroup;
        [SerializeField] private CanvasGroup _thirdContinueButtonGroup;
        [SerializeField] private CanvasGroup _fourthContinueButtonGroup;
        [Space] 
        [SerializeField] private Button _skipButton;

        private CancellationTokenSource _cts;

        private void OnEnable()
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(this.destroyCancellationToken);
            _cts.Token.Register(() =>
            {
                if (_canvasGroup)
                    _canvasGroup.DOFade(0, 0.2f)
                        .SetTarget(gameObject)
                        .OnComplete(() => gameObject.SetActive(false));
            });
            
            _skipButton.onClick.AddListener(OnSkipClick);
            ApplicationState.SetPauseGameplay(true);
        }

        private void OnDisable()
        {
            ApplicationState.SetPauseGameplay(false);
            _skipButton.onClick.RemoveListener(OnSkipClick);
            DOTween.Complete(gameObject);
        }
        private void OnSkipClick() => _cts.Cancel();

        public async UniTask StartLearn(bool withSkip)
        {
            _skipButton.gameObject.SetActive(withSkip);
            
            _firstMask.gameObject.SetActive(false);
            _secondMask.gameObject.SetActive(false);
            _thirdMask.gameObject.SetActive(false);
            _fourthMask.gameObject.SetActive(false);
            
            _firstText.gameObject.SetActive(false);
            _secondText.gameObject.SetActive(false);
            _thirdText.gameObject.SetActive(false);
            _fourthText.gameObject.SetActive(false);
            
            _firstContinueButton.gameObject.SetActive(false);
            _secondContinueButton.gameObject.SetActive(false);
            _thirdContinueButton.gameObject.SetActive(false);
            _fourthContinueButton.gameObject.SetActive(false);
            _canvasGroup.alpha = 1;
            
            await _black.DOFade(0.9f, 0.3f).From(0)
                .SetTarget(gameObject)
                .ToUniTask(cancellationToken: _cts.Token, tweenCancelBehaviour: TweenCancelBehaviour.KillAndCancelAwait)
                .SuppressCancellationThrow();
            if (_cts.IsCancellationRequested)
                return;
            
            await ShowPart(_firstMask, _firstText, _firstContinueButton, _firstContinueButtonGroup);
            if (_cts.IsCancellationRequested)
                return;
            await ShowPart(_secondMask, _secondText, _secondContinueButton, _secondContinueButtonGroup);
            if (_cts.IsCancellationRequested)
                return;
            await ShowPart(_thirdMask, _thirdText, _thirdContinueButton, _thirdContinueButtonGroup);
            if (_cts.IsCancellationRequested)
                return;
            await ShowPart(_fourthMask, _fourthText, _fourthContinueButton, _fourthContinueButtonGroup);
            if (_cts.IsCancellationRequested)
                return;
            
            await _canvasGroup.DOFade(0, 0.2f)
                .SetTarget(gameObject)
                .OnComplete(() => gameObject.SetActive(false))
                .ToUniTask(cancellationToken: _cts.Token, tweenCancelBehaviour: TweenCancelBehaviour.KillAndCancelAwait)
                .SuppressCancellationThrow();
        }

        private async UniTask ShowPart(RectTransform mask, TMP_Text text, Button continueButton, CanvasGroup buttonCanvas)
        {
            await mask.DOScale(1, 0.5f).From(0)
                .OnStart(() => mask.gameObject.SetActive(true))
                .SetTarget(gameObject)
                .ToUniTask(cancellationToken: _cts.Token, tweenCancelBehaviour: TweenCancelBehaviour.KillAndCancelAwait)
                .SuppressCancellationThrow();
            if (_cts.IsCancellationRequested)
                return;
            var textPos = text.rectTransform.anchoredPosition;
            textPos.y -= 100;
            await DOTween.Sequence(gameObject)
                .OnStart(() => text.gameObject.SetActive(true))
                .Append(text.rectTransform.DOAnchorPos(text.rectTransform.anchoredPosition,0.5f).From(textPos))
                .Join(text.DOFade(1, 0.3f).From(0))
                .ToUniTask(cancellationToken: _cts.Token, tweenCancelBehaviour: TweenCancelBehaviour.KillAndCancelAwait)
                .SuppressCancellationThrow();
            
            if (_cts.IsCancellationRequested)
                return;
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: _cts.Token)
                .SuppressCancellationThrow();
            if (_cts.IsCancellationRequested)
                return;
            
            await buttonCanvas.DOFade(1, 0.3f).From(0)
                .OnStart(() => continueButton.gameObject.SetActive(true))
                .SetTarget(gameObject)
                .ToUniTask(cancellationToken: _cts.Token, tweenCancelBehaviour: TweenCancelBehaviour.KillAndCancelAwait)
                .SuppressCancellationThrow();
            if (_cts.IsCancellationRequested)
                return;
            await continueButton.OnClickAsync();
            if (_cts.IsCancellationRequested)
                return;
            continueButton.gameObject.SetActive(false);
            await DOTween.Sequence(gameObject)
                .OnComplete(() =>
                {
                    mask.gameObject.SetActive(false);
                    text.gameObject.SetActive(false);
                })
                .Append(mask.DOScale(0, 0.2f))
                .Join(text.DOFade(1, 0.2f))
                .ToUniTask(cancellationToken: _cts.Token, tweenCancelBehaviour: TweenCancelBehaviour.KillAndCancelAwait)
                .SuppressCancellationThrow();
        }
    }
}