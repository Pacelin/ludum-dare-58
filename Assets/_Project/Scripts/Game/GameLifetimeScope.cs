using Scripts.Core.UI;
using Scripts.Game.Currency;
using Scripts.Game.Flowers;
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
        [SerializeField] private DrawCollider _drawFieldPrefab;
        [SerializeField] private int _initialWalletAmount = 10;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(_pauseDialogViewPrefab, Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab(_settingsDialogViewPrefab, Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab(_gameScreenViewPrefab, Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab(_drawFieldPrefab, Lifetime.Singleton);
            builder.Register<DrawFacade>(Lifetime.Singleton);
            builder.Register<Wallet>(Lifetime.Singleton).WithParameter(_initialWalletAmount);
            builder.RegisterEntryPoint<PauseDialogController>();
            builder.RegisterEntryPoint<GameScreenPresenter>();
        }
    }
}