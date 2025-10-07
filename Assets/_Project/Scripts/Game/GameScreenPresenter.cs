using System;
using R3;
using Scripts.Core.Lifetime;
using Scripts.Game.Butterflies;
using Scripts.Game.Butterflies.ButterfliesJournal;
using Scripts.Game.Butterflies.ButterfliesJournal.Legend;
using Scripts.Game.Currency;
using Scripts.Game.Flowers;
using VContainer.Unity;

namespace Scripts.Game
{
    public class GameScreenPresenter : IInitializable, IDisposable
    {
        private readonly GameScreenView _screen;
        private readonly WalletPresenter _walletPresenter;
        private readonly HotbarController _hotbarController;
        private readonly ButterfliesLegendPresenter _legendPresenter;
        private readonly ButterfliesJournalView _butterfliesJournalView;
        private readonly CompositeDisposable _disposables;
        
        public GameScreenPresenter(GameScreenView screen, DrawFacade drawFacade, Wallet wallet,
            ButterfliesJournalView journalView, ButterfliesJournal journal, ButterfliesConfig butterfliesConfig)
        {
            _screen = screen;
            _butterfliesJournalView = journalView;
            _walletPresenter = new WalletPresenter(screen.WalletView, wallet);
            _hotbarController = new HotbarController(drawFacade, screen.Hotbar, wallet);
            _legendPresenter = new ButterfliesLegendPresenter(butterfliesConfig, _screen.Legend, journal, journalView);
            _disposables = new CompositeDisposable();
        }

        public void Initialize()
        {
            _screen.PauseButton.OnClickAsObservable()
                .Subscribe(_ => ApplicationState.SetPause(true))
                .AddTo(_disposables);
            _screen.JournalButton.OnClickAsObservable()
                .Subscribe(_ => _butterfliesJournalView.Open(0))
                .AddTo(_disposables);
            _walletPresenter.Initialize();
            _hotbarController.Initialize();
            _legendPresenter.Initialize();
        }

        public void Dispose()
        {
            _walletPresenter.Dispose();
            _hotbarController.Dispose();
            _legendPresenter.Dispose();
            _disposables.Dispose();
        }
    }
}