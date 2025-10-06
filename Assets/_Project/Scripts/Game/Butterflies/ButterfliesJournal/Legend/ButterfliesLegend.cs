using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.Butterflies.ButterfliesJournal.Legend
{
    public class ButterfliesLegend : MonoBehaviour
    {
        [SerializeField] private RectTransform _elementsContainer;
        [SerializeField] private ButterfliesLegendElement _legendElementPrefab;

        public ButterfliesLegendElement CreateLegendElement() =>
            Instantiate(_legendElementPrefab, _elementsContainer);
        
        public void UpdateLayout() => LayoutRebuilder.ForceRebuildLayoutImmediate(_elementsContainer);
    }
}