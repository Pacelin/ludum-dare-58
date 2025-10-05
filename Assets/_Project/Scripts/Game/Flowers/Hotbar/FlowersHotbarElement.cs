using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.Flowers
{
    public class FlowersHotbarElement : HotbarElement
    {
        public Flower Flower => _flower;
        public Button UnlockButton => _unlockButton;
        
        [SerializeField] private Flower _flower;
        [Space]
        [SerializeField] private Button _unlockButton;
        [SerializeField] private GameObject _whenUnlocked;
        [SerializeField] private GameObject _whenHavePrice;
        [SerializeField] private GameObject _whenLocked;
        [SerializeField] private GameObject _whenNotEnoughCoins;
        [SerializeField] private TMP_Text _unlockCoinsText;
        [SerializeField] private TMP_Text _drawCoinsText;

        public void SetUnlockCoins(int unlockCoins) => _unlockCoinsText.text = unlockCoins.ToString();

        public void SetDrawCoins(int drawCoins)
        {
            _drawCoinsText.text = drawCoins.ToString();
            _whenHavePrice.SetActive(drawCoins > 0);
        }
        
        public void SetState(bool isUnlocked, bool isEnoughCoins)
        {
            Button.interactable = isUnlocked;
            _whenUnlocked.SetActive(isUnlocked);
            _whenLocked.SetActive(!isUnlocked);
            _whenNotEnoughCoins.SetActive(!isUnlocked && !isEnoughCoins);
        }

        public override void Visit(HotbarController hotbarController) => hotbarController.Accept(this);
    }
}