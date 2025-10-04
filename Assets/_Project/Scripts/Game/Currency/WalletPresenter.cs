using System;
using R3;
using VContainer.Unity;

namespace Scripts.Game.Currency
{
    public class WalletPresenter : IInitializable, IDisposable
    {
        private readonly WalletView _view;
        private readonly Wallet _model;
        private readonly CompositeDisposable _disposables;
        
        public WalletPresenter(WalletView view, Wallet model)
        {
            _view = view;
            _model = model;
            _disposables = new CompositeDisposable();
        }

        public void Initialize()
        {
            _model.Balance.Subscribe(_view.SetBalance).AddTo(_disposables);
            _model.ObserveEarn().Subscribe(_ => _view.DOEarn()).AddTo(_disposables);
            _model.ObserveSpend().Subscribe(_ => _view.DOSpend()).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}