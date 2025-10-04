using Scripts.Core.UI;
using Scripts.Game.Currency;
using Scripts.Game.Flowers;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game
{
    public class GameScreenView : ScreenView
    {
        public Button PauseButton => _pauseButton;
        public WalletView WalletView => _walletView;
        public Hotbar Hotbar => _hotbar;
        
        [SerializeField] private Button _pauseButton;
        [SerializeField] private WalletView _walletView;
        [SerializeField] private Hotbar _hotbar;
    }
}