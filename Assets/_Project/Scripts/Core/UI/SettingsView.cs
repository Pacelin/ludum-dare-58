using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Core.UI
{
    public class SettingsView : MonoBehaviour
    {
        public Slider MasterVolume => _masterVolume;
        public Slider MusicVolume => _musicVolume;
        public Slider SoundVolume => _soundVolume;
        
        [SerializeField] private Slider _masterVolume;
        [SerializeField] private Slider _musicVolume;
        [SerializeField] private Slider _soundVolume;
    }
}