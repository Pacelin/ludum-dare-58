using System.Threading;
using Cysharp.Threading.Tasks;
using Scripts.Core.InspectorCustomization;
using Scripts.Core.Lifetime;
using Scripts.Core.UnityEditorHelpers;
using UnityEngine;
using VContainer;

namespace Scripts.Audio
{
    [CreateAsset("SO_Installer_FMOD")]
    public class FMODInstaller : ApplicationInstaller
    {
        [DrawInBox(ShowLabel = true)]
        [SerializeField] private AudioVolumes _volumes;
        
        public override UniTask Load(CancellationToken cancellationToken)
        {
            return AudioSystem.Initialize(_volumes, cancellationToken);
        }

        public override void Install(IContainerBuilder builder)
        {
        }
    }
}