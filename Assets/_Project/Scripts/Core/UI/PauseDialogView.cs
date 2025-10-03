using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Core.UI
{
    public class PauseDialogView : DialogView
    {
        public Button BackButton => _backButton;
        public Button RestartButton => _restartButton;
        public Button ExitButton => _exitButton;
        public Button SettingsButton => _settingsButton;

        [SerializeField] private Button _backButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;
    }
}