using System.Threading;
using R3;
using UnityEngine;
using UnityEngine.Profiling;

namespace Scripts.Core.Lifetime
{
    public static class ApplicationState
    {
        public static CancellationToken ExitCancellationToken => Application.exitCancellationToken;
        public static ReadOnlyReactiveProperty<bool> IsPaused => _monoProvider.IsPaused;
        public static ReadOnlyReactiveProperty<bool> IsPausedByUser => _monoProvider.IsPausedByUser;
        public static ReadOnlyReactiveProperty<bool> HasFocus => _monoProvider.HasFocus;

        private static ApplicationMonoProvider _monoProvider;

        internal static void Initialize(ApplicationMonoProvider monoProvider) => _monoProvider = monoProvider;

        public static void SetPause(bool pause) => _monoProvider.SetPause(pause);
        public static void SetPauseGameplay(bool pause)
        {
            _monoProvider.SetPauseGameplay(pause);
        }
    }
}