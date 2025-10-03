using Scripts.Core.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scripts.Game
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private PauseDialogView _pauseDialogViewPrefab;
        [SerializeField] private SettingsDialogView _settingsDialogViewPrefab;
        [SerializeField] private GameScreenView _gameScreenViewPrefab;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(_pauseDialogViewPrefab, Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab(_settingsDialogViewPrefab, Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab(_gameScreenViewPrefab, Lifetime.Singleton);
            builder.RegisterEntryPoint<PauseDialogController>();
            builder.RegisterEntryPoint<GameScreenPresenter>();
        }
    }
}