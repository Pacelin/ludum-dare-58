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
            _focusDisposable = ApplicationState.HasFocus.Skip(1).DistinctUntilChanged().Subscribe(hasFocus => {
                if (RuntimeManager.StudioSystem.isValid())
                {
                    RuntimeManager.PauseAllEvents(!hasFocus);
                    
                    if (hasFocus)
                        RuntimeManager.CoreSystem.mixerResume();
                    else
                        RuntimeManager.CoreSystem.mixerSuspend();
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