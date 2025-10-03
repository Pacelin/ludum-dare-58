using Scripts.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game
{
    public class GameScreenView : ScreenView
    {
        public Button PauseButton => _pauseButton;
        
        [SerializeField] private Button _pauseButton;
    }
}