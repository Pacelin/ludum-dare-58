using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.Butterflies.ButterfliesJournal.Legend
{
    public class ButterfliesLegendElement : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _butterflyIcon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _sizeText;
        [SerializeField] private GameObject _newRecordObject;
        [SerializeField] private GameObject _newWorldRecordObject;
        [SerializeField] private GameObject _newSpeciesObject;

        public void Initialize(ButterfliesLegend legend, ButterfliesConfig generalConfig, ButterflyView butterfly)
        {
            //_butterflyIcon.sprite = butterfly.SpriteRenderer.sprite;
            //_nameText.text = butterfly.Config.Name.GetLocalizedString();
            //_sizeText.text = $"{butterfly.Size:F} mm";
            //_costText.text = $"{ButterfliesUtils.CalculateCost(generalConfig, butterfly.Config, butterfly.Size)}";
        }
    }
}