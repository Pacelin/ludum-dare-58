using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.Butterflies.ButterfliesJournal.Legend
{
    public class ButterfliesLegend : MonoBehaviour
    {
        [SerializeField] private RectTransform _elementsContainer;
        [SerializeField] private ButterfliesLegendElement _legendElementPrefab;

        public void PostButterfly(ButterfliesConfig generalConfig, ButterflyView butterfly)
        {
            //var element = Instantiate(_legendElementPrefab, _elementsContainer);
            //element.Initialize(this, generalConfig, butterfly);
        }
        
        public void UpdateLayout() => LayoutRebuilder.ForceRebuildLayoutImmediate(_elementsContainer);
    }
}