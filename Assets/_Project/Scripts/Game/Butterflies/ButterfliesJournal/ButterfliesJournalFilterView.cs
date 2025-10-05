using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.Butterflies.ButterfliesJournal
{
    public class ButterfliesJournalFilterView : MonoBehaviour
    {
        public Button Button => _button;
        public ButterfliesJournalPageView[] Pages => _pages;
        
        [SerializeField] private Button _button;
        [SerializeField] private ButterfliesJournalPageView[] _pages;
        [Space]
        [SerializeField] private RectTransform _moveContainer;
        [SerializeField] private Vector2 _defaultAnchoredPosition;
        [SerializeField] private Vector2 _selectedAnchoredPosition;

        private void OnDisable() => _moveContainer.DOComplete();

        public void UpdateSelection(bool isSelected) =>
            _moveContainer.DOAnchorPos(isSelected ? _selectedAnchoredPosition : _defaultAnchoredPosition, 0.2f);
        public void ResetSelection(bool isSelected) =>
            _moveContainer.anchoredPosition = isSelected ? _selectedAnchoredPosition : _defaultAnchoredPosition;
    }
}