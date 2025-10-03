using System;
using Jammer;
using JetBrains.Annotations;
using mixpanel;
using R3;
using VContainer.Unity;

namespace Scripts.Analytics
{
    [UsedImplicitly]
    public class AnalyticsHandler : IInitializable, IDisposable
    {
        private IDisposable _disposable;
        private int _seconds;
        
        public void Initialize()
        {
            Mixpanel.SetPreferencesSource(new ReplaceWithLocalStorage());
            Mixpanel.Track("@session_start");
            Mixpanel.Flush();

            _disposable = Observable
                .Interval(TimeSpan.FromSeconds(30), UnityTimeProvider.TimeUpdateRealtime)
                .Skip(1)
                .Subscribe(_ =>
                {
                    _seconds += 30;
                    Mixpanel.Track("playtime", "Seconds", new Value(_seconds));
                    Mixpanel.Flush();
                });
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}