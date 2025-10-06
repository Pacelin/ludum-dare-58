using DG.Tweening;
using R3;
using Scripts.Core.Lifetime;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.Butterflies.ButterfliesJournal
{
    public class ButterfliesJournalView : MonoBehaviour
    {
        public RectTransform FirstPageContainer => _firstPageContainer;
        public RectTransform SecondPageContainer => _secondPageContainer;
        public Button LeftArrow => _leftArrow;
        public Button RightArrow => _rightArrow;
        public Button CloseButton => _closeButton;
        
        public ButterfliesJournalFilterView[] Filters => _filters;

        [SerializeField] private Image _block;
        [SerializeField] private RectTransform _scaleRoot;
        [Space]
        [SerializeField] private RectTransform _firstPageContainer;
        [SerializeField] private RectTransform _secondPageContainer;
        [SerializeField] private Button _leftArrow;
        [SerializeField] private Button _rightArrow;
        [SerializeField] private Button _closeButton;
        [SerializeField] private ButterfliesJournalFilterView[] _filters;
        
        private readonly Subject<int> _openSubject = new Subject<int>();
        
        public Observable<int> ObserveOpen() => _openSubject;

        private void OnEnable()
        {
            ApplicationState.SetPauseGameplay(true);
        }

        private void OnDisable()
        {
            ApplicationState.SetPauseGameplay(false);
            _scaleRoot.DOKill();
            _block.DOKill();
        } 

        public void Open(int butterflyId)
        {
            _openSubject.OnNext(butterflyId);
            _block.DOFade(0.8f, 0.2f).From(0);
            _scaleRoot.DOScale(1, 0.2f)
                .From(0)
                .OnStart(() => gameObject.SetActive(true));
        }

        public void Close()
        {
            _block.DOFade(0, 0.2f);
            _scaleRoot.DOScale(0, 0.2f)
                .From(1)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}