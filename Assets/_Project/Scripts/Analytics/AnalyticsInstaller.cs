using System.Threading;
using Cysharp.Threading.Tasks;
using mixpanel;
using Scripts.Core.Lifetime;
using Scripts.Core.UnityEditorHelpers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scripts.Analytics
{
    [CreateAsset("SO_Installer_Analytics")]
    public class AnalyticsInstaller : ApplicationInstaller
    {
        public override async UniTask Load(CancellationToken cancellationToken)
        {
            try
            {
                if (Mixpanel.IsInitialized())
                    return;
                Mixpanel.Init();
                await UniTask.WaitUntil(() => Mixpanel.IsInitialized(), cancellationToken: cancellationToken)
                    .SuppressCancellationThrow();
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }

        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<AnalyticsHandler>();
        }
    }
}