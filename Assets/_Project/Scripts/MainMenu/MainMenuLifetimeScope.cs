using Scripts.Core.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scripts.MainMenu
{
    public class MainMenuLifetimeScope : LifetimeScope
    {
        [SerializeField] private MainMenuScreenView _screenPrefab;
        [SerializeField] private SettingsDialogView _settingsDialogPrefab;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(_screenPrefab, Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab(_settingsDialogPrefab, Lifetime.Singleton);
            builder.RegisterEntryPoint<MainMenuScreenPresenter>();
        }
    }
}