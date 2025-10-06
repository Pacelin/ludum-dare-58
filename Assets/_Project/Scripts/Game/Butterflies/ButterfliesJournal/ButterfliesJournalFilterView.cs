using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Game.Butterflies.ButterfliesJournal
{
    public class ButterfliesJournalFilterView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Observable<Unit> Click => _clickSubject;
        public ButterfliesJournalPageView[] Pages => _pages;
        
        [SerializeField] private ButterfliesJournalPageView[] _pages;
        [Space]
        [SerializeField] private RectTransform _sizeContainer;
        [SerializeField] private Vector2 _defaultSizeDelta;
        [SerializeField] private Vector2 _selectedSizeDelta;
        [SerializeField] private Vector2 _hoverSizeDelta;

        private Subject<Unit> _clickSubject = new Subject<Unit>();
        private bool _isSelected;

        private void OnDisable()
        {
            _sizeContainer.DOKill();
            if (!_isSelected)
                _sizeContainer.sizeDelta = _defaultSizeDelta;
            else
                _sizeContainer.sizeDelta = _selectedSizeDelta;
        } 

        public void UpdateSelection(bool isSelected)
        {
            _sizeContainer.DOKill();
            _sizeContainer.DOSizeDelta(isSelected ? _selectedSizeDelta : _defaultSizeDelta, 0.1f);
            _isSelected = isSelected;
        }

        public void ResetSelection(bool isSelected)
        {
            _sizeContainer.DOKill();
            _sizeContainer.sizeDelta = isSelected ? _selectedSizeDelta : _defaultSizeDelta;
            _isSelected = isSelected;
        }

        public void OnPointerClick(PointerEventData eventData) => _clickSubject.OnNext(Unit.Default);

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isSelected) return;
            
            _sizeContainer.DOKill();
            _sizeContainer.DOSizeDelta(_hoverSizeDelta, 0.1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isSelected) return;
            
            _sizeContainer.DOKill();
            _sizeContainer.DOSizeDelta(_defaultSizeDelta, 0.1f);
        }
    }
}