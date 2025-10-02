using System.Threading;
using Cysharp.Threading.Tasks;
using Scripts.Core.Lifetime;
using Scripts.Core.UnityEditorHelpers;
using VContainer;
using VContainer.Unity;

namespace Scripts.Loading
{
    [CreateAsset("SO_Installer_Loading")]
    public class ApplicationLoadingInstaller : ApplicationInstaller
    {
        public override UniTask Load(CancellationToken cancellationToken) => UniTask.CompletedTask;

        public override void Install(IContainerBuilder builder) => builder.RegisterEntryPoint<ApplicationLoading>();
    }
}