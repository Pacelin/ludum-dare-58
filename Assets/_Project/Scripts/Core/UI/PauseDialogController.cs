using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using R3;
using Scripts.Core.Lifetime;
using Scripts.Core.SceneManagement;
using VContainer.Unity;

namespace Scripts.Core.UI
{
    [UsedImplicitly]
    public class PauseDialogController : IInitializable, IDisposable
    {
        private readonly PauseDialogView _pauseDialogView;
        private readonly CompositeDisposable _disposables;
        private readonly SettingsDialogView _settingsDialog;
        
        public PauseDialogController(PauseDialogView pauseDialogView, SettingsDialogView settingsDialog)
        {
            _pauseDialogView = pauseDialogView;
            _settingsDialog = settingsDialog;
            _disposables = new CompositeDisposable();
        }
        
        public void Initialize()
        {
            _pauseDialogView.gameObject.SetActive(false);
            ApplicationState.IsPausedByUser.Skip(1).DistinctUntilChanged().Subscribe(isPaused =>
            {
                if (isPaused)
                    _pauseDialogView.Open(ApplicationState.ExitCancellationToken).Forget();
                else
                    _pauseDialogView.Close(ApplicationState.ExitCancellationToken).Forget();
            }).AddTo(_disposables);
            
            _pauseDialogView.BackButton.OnClickAsObservable()
                .Subscribe(_ => ApplicationState.SetPause(false))
                .AddTo(_disposables);
            _pauseDialogView.RestartButton.OnClickAsObservable()
                .Subscribe(_ => SceneManager.LoadScene(SceneManager.Database.Game).Forget())
                .AddTo(_disposables);
            _pauseDialogView.ExitButton.OnClickAsObservable()
                .Subscribe(_ => SceneManager.LoadScene(SceneManager.Database.MainMenu).Forget())
                .AddTo(_disposables);
            _pauseDialogView.SettingsButton.OnClickAsObservable()
                .Subscribe(_ => _settingsDialog.Open(ApplicationState.ExitCancellationToken))
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}