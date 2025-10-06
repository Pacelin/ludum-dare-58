using System;
using Cysharp.Threading.Tasks;
using R3;
using VContainer.Unity;

namespace Scripts.Game
{
    public class GameLearnPresenter : IInitializable, IDisposable
    {
        private readonly GameScreenView _gameScreenView;
        private readonly GameLearnView _learnView;
        private readonly CompositeDisposable _disposables;
        
        public GameLearnPresenter(GameScreenView gameScreenView, GameLearnView learnView)
        {
            _gameScreenView = gameScreenView;
            _learnView = learnView;
            _disposables = new CompositeDisposable();
        }
        
        public void Initialize()
        {
            _gameScreenView.LearnButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _learnView.gameObject.SetActive(true);
                    _learnView.StartLearn(true).Forget();
                })
                .AddTo(_disposables);

            var alreadyComplete = UserDataManager.GetInt("learned", 0) == 1;
            _learnView.gameObject.SetActive(!alreadyComplete);
            if (!alreadyComplete)
            {
                UserDataManager.SetInt("learned", 1);
                _learnView.StartLearn(false).Forget();
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}