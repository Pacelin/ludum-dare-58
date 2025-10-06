using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using R3;
using Scripts.Audio;
using Scripts.Core.Lifetime;
using Scripts.Core.SceneManagement;
using Scripts.Core.UI;
using UnityEngine;
using VContainer.Unity;

namespace Scripts.MainMenu
{
    [UsedImplicitly]
    public class MainMenuScreenPresenter : IInitializable, IDisposable
    {
        private readonly MainMenuScreenView _screen;
        private readonly SettingsDialogView _settingsDialog;
        private readonly CompositeDisposable _disposables;

        private SoundEventInstance _music;

        public MainMenuScreenPresenter(MainMenuScreenView screen, SettingsDialogView settingsDialog)
        {
            _screen = screen;
            _settingsDialog = settingsDialog;
            _disposables = new CompositeDisposable();
        } 
        
        public void Initialize()
        {
            _music = AudioSystem.Game_MainMenuMusic.CreateInstance();
            _music.Start();
            
            _screen.PlayButton.OnClickAsObservable()
                .Subscribe(_ => SceneManager.LoadScene(SceneManager.Database.Game).Forget())
                .AddTo(_disposables);
            _screen.SettingsButton.OnClickAsObservable()
                .Subscribe(_ => _settingsDialog.Open(ApplicationState.ExitCancellationToken).Forget())
                .AddTo(_disposables);

            if (Application.platform == RuntimePlatform.WebGLPlayer)
                _screen.ExitButton.gameObject.SetActive(false);
            else
                _screen.ExitButton.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.ExitPlaymode();
#else
                        Application.Quit();
#endif
                    })
                    .AddTo(_disposables);
        }

        public void Dispose()
        {
            _music.Stop(true);
            _music.Release();
            _disposables.Dispose();
        }
    }
}