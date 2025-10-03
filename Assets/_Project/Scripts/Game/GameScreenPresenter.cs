using System;
using R3;
using Scripts.Core.Lifetime;
using VContainer.Unity;

namespace Scripts.Game
{
    public class GameScreenPresenter : IInitializable, IDisposable
    {
        private readonly GameScreenView _screen;
        private readonly CompositeDisposable _disposables;
        
        public GameScreenPresenter(GameScreenView screen)
        {
            _screen = screen;
            _disposables = new CompositeDisposable();
        }

        public void Initialize()
        {
            _screen.PauseButton.OnClickAsObservable()
                .Subscribe(_ => ApplicationState.SetPause(true)).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}