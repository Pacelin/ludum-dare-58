using Scripts.Core.UI;
using Scripts.Game.Butterflies.ButterfliesJournal.Legend;
using Scripts.Game.Currency;
using Scripts.Game.Flowers;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game
{
    public class GameScreenView : ScreenView
    {
        public Button PauseButton => _pauseButton;
        public Button JournalButton => _journalButton;
        public WalletView WalletView => _walletView;
        public Hotbar Hotbar => _hotbar;
        public ButterfliesLegend Legend => _legend;
        
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _journalButton;
        [SerializeField] private WalletView _walletView;
        [SerializeField] private Hotbar _hotbar;
        [SerializeField] private ButterfliesLegend _legend;
    }
}