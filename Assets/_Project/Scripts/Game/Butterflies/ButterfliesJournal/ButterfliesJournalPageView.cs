using UnityEngine;

namespace Scripts.Game.Butterflies.ButterfliesJournal
{
    public class ButterfliesJournalPageView : MonoBehaviour
    {
        public ButterfliesJournalEntryView[] Entries => _entries;

        [SerializeField] private RectTransform _origin;
        [SerializeField] private ButterfliesJournalEntryView[] _entries;

        public void Assign(RectTransform container)
        {
            _origin.anchorMin = container.anchorMin;
            _origin.anchorMax = container.anchorMax;
            _origin.anchoredPosition = container.anchoredPosition;
            gameObject.SetActive(true);
        }
        
        public void Deassign() => gameObject.SetActive(false);
    }
}