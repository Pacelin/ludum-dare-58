using System;
using FMODUnity;
using JetBrains.Annotations;
using Scripts.Core.Lifetime;
using VContainer.Unity;
using R3;

namespace Scripts.Audio
{
    [UsedImplicitly]
    public class FMODPauseManager : IInitializable, IDisposable
    {
        private IDisposable _focusDisposable;
        
        public void Initialize()
        {
            _focusDisposable = ApplicationState.IsPaused.Subscribe(isPaused => {
                if (RuntimeManager.StudioSystem.isValid())
                {
                    RuntimeManager.PauseAllEvents(isPaused);
                    
                    if (isPaused)
                        RuntimeManager.CoreSystem.mixerSuspend();
                    else
                        RuntimeManager.CoreSystem.mixerResume();
                }
            });
        }

        public void Dispose()
        {
            if (RuntimeManager.StudioSystem.isValid())
                RuntimeManager.CoreSystem.mixerResume();
            _focusDisposable.Dispose();
        }
    }
}