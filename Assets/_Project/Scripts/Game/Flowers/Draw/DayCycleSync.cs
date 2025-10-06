using System;
using R3;
using Scripts.Audio;
using Scripts.Core.Lifetime;
using VContainer.Unity;

namespace Scripts.Game.Flowers
{
    public class DayCycleSync : IInitializable, IDisposable, ITickable
    {
        private readonly DrawCollider _drawCollider;
        private readonly GameTime _gameTime;
        private readonly CompositeDisposable _disposables;
        
        private SoundEvent_Game_GameMusic.Instance _musicInstance;
        private SoundEventInstance _pauseInstance;
        
        public DayCycleSync(DrawCollider drawCollider, GameTime gameTime)
        {
            _drawCollider = drawCollider;
            _gameTime = gameTime;
            _disposables = new CompositeDisposable();
        }

        public void Tick()
        {
            _drawCollider.DrawField.DayCycleSystem.SetCycleTime(_gameTime.Seconds.CurrentValue);
        }

        public void Initialize()
        {
            _musicInstance = AudioSystem.Game_GameMusic.CreateInstance();
            _musicInstance.Start();
            
            ApplicationState.IsPaused.DistinctUntilChanged().Subscribe(isPaused =>
            {
                _musicInstance.SetPaused(isPaused);
                if (isPaused)
                {
                    if (_gameTime.IsNight)
                        _pauseInstance = AudioSystem.Game_PauseMusicNight.CreateInstance();
                    else
                        _pauseInstance = AudioSystem.Game_PauseMusicDay.CreateInstance();
                    _pauseInstance.Start();
                }
                else
                {
                    if (_pauseInstance != null)
                    {
                        _pauseInstance.Stop(true);
                        _pauseInstance.Release();
                        _pauseInstance = null;
                    }
                }
            }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _musicInstance.Stop(true);
            _musicInstance.Release();
            if (_pauseInstance != null)
            {
                _pauseInstance.Stop(true);
                _pauseInstance.Release();
            }
        }
    }
}