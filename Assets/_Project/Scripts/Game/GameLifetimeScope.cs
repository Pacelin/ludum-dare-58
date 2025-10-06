using Scripts.Core.UI;
using Scripts.Game.Butterflies;
using Scripts.Game.Butterflies.ButterfliesJournal;
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
        [SerializeField] private ButterfliesConfig _butterfliesConfig;
        [SerializeField] private ButterfliesJournalView _journalViewPrefab;
        [SerializeField] private GameLearnView _learnViewPrefab;
        [SerializeField] private int _initialWalletAmount = 10;
        [SerializeField] private GameTime.Config _gameTimeConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(_pauseDialogViewPrefab, Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab(_settingsDialogViewPrefab, Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab(_gameScreenViewPrefab, Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab(_drawFieldPrefab, Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab(_journalViewPrefab, Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab(_learnViewPrefab, Lifetime.Singleton);
            
            builder.RegisterInstance<ButterfliesConfig>(_butterfliesConfig);
            builder.RegisterInstance<GameTime.Config>(_gameTimeConfig);
            builder.Register<DrawFacade>(Lifetime.Singleton);
            builder.Register<Wallet>(Lifetime.Singleton).WithParameter(_initialWalletAmount);
            builder.Register<ButterfliesJournal>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<PauseDialogController>();
            builder.RegisterEntryPoint<GameScreenPresenter>();
            builder.RegisterEntryPoint<ButterflyConfig>();
            builder.RegisterEntryPoint<GameTime>().AsSelf();
            builder.RegisterEntryPoint<ButterfliesSpawner>();
            builder.RegisterEntryPoint<ButterfliesJournalPresenter>();
            builder.RegisterEntryPoint<GameLearnPresenter>();
            builder.Register<ButterfliesCatcher>(Lifetime.Singleton);
        }
    }
}