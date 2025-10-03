using Scripts.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.MainMenu
{
    public class MainMenuScreenView : ScreenView
    {
        public Button PlayButton => _playButton;
        public Button SettingsButton => _settingsButton;
        public Button ExitButton => _exitButton;
        
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;
    }
}