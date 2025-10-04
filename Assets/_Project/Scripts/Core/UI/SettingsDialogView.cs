using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using Scripts.Audio;
using Scripts.Core.Lifetime;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Core.UI
{
    public class SettingsDialogView : DialogView
    {
        [SerializeField] private SettingsView _settings;
        [SerializeField] private Button[] _closeButtons;
        
        private CompositeDisposable _disposables;
        
        private void OnEnable()
        {
            _disposables = new CompositeDisposable();
            _settings.MasterVolume.value = AudioSystem.Volumes.MasterVolume;
            _settings.MusicVolume.value = AudioSystem.Volumes.GetVolume(0);
            _settings.SoundVolume.value = AudioSystem.Volumes.GetVolume(1);
            
            _settings.MasterVolume.OnValueChangedAsObservable()
                .Subscribe(v =>
                {
                    AudioSystem.UI_Slider.PlayOneShot();
                    AudioSystem.Volumes.MasterVolume = v;
                })
                .AddTo(_disposables);
            
            _settings.MusicVolume.OnValueChangedAsObservable()
                .Subscribe(v =>
                {
                    AudioSystem.UI_Slider.PlayOneShot();
                    AudioSystem.Volumes.SetVolume(0, v);
                })
                .AddTo(_disposables);
            
            _settings.SoundVolume.OnValueChangedAsObservable()
                .Subscribe(v =>
                {
                    AudioSystem.UI_Slider.PlayOneShot();
                    AudioSystem.Volumes.SetVolume(1, v);
                })
                .AddTo(_disposables);
            
            foreach (var button in _closeButtons)
                button.OnClickAsObservable()
                    .Subscribe(_ => Close(ApplicationState.ExitCancellationToken).Forget())
                    .AddTo(_disposables);
        }

        protected override void OnDisable()
        {
            _disposables.Dispose();
            _disposables = null;
            base.OnDisable();
        }
    }
}