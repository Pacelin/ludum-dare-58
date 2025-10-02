using System.Threading;
using Cysharp.Threading.Tasks;
using Scripts.Core.InspectorCustomization;
using Scripts.Core.Lifetime;
using Scripts.Core.UnityEditorHelpers;
using UnityEngine;
using VContainer;

namespace Scripts.Core.SceneManagement
{
    [CreateAsset("SO_Installer_SceneManagement")]
    public class SceneManagementInstaller : ApplicationInstaller
    {
        [DrawInBox(ShowLabel = false)]
        [SerializeField] private ScenesData _scenesData;

        public override UniTask Load(CancellationToken cancellationToken)
        {
            SceneManager.Initialize(_scenesData);
            return UniTask.CompletedTask;
        }

        public override void Install(IContainerBuilder builder) { }
    }
}