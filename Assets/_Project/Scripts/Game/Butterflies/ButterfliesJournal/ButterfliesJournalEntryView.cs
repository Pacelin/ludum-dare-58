using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.Butterflies.ButterfliesJournal
{
    public class ButterfliesJournalEntryView : MonoBehaviour
    {
        public ButterflyView Butterfly => _butterfly;
        public GameObject QuestionMark => _questionMark;
        
        [SerializeField] private GameObject _questionMark;
        [SerializeField] private ButterflyView _butterfly;
        [Space]
        [SerializeField] private RectTransform _butterflyContainer;
        [SerializeField] private float _scaleAdapter = 1f;
        [SerializeField] private float _scaleOffset = 0f;
        [Space] 
        [SerializeField] private GameObject _worldInfo;
        [Space]
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _mySizeText;
        [SerializeField] private TMP_Text _myCostText;
        [SerializeField] private TMP_Text _worldSizeText;
        [SerializeField] private TMP_Text _worldCostText;
        [SerializeField] private TMP_Text _worldNicknameText;

        private Image _image;
        
        public void SetData(ButterfliesConfig butterfliesConfig,
            float mySize, int myCost)
        {
            var scale = butterfliesConfig.GetScaleForJournal(mySize) * _scaleAdapter + _scaleOffset;
            _butterflyContainer.localScale = Vector3.one * scale;
            _nameText.text = ButterfliesUtils.GetButterflyName(butterfliesConfig, _butterfly.Config, mySize);
            _mySizeText.text = mySize.ToString("F2") + " mm";
            _myCostText.text = myCost.ToString();
        }

        public Material GetMaterial()
        {
            if (!_image) 
                _image = _butterflyContainer.GetComponent<Image>();
            return _image.material;
        } 
        
        public void ResetWorld() => _worldInfo.SetActive(false);

        public void SetWorldData(float worldSize, int worldCost, string worldNickname)
        {
            _worldInfo.SetActive(true);
            _worldSizeText.text = worldSize.ToString("F2") + " mm";
            _worldCostText.text = worldCost.ToString();
            _worldNicknameText.text = worldNickname;
        }
    }
}