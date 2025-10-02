using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Scripts.Core.Lifetime
{
    public abstract class ApplicationInstaller : ScriptableObject
    {
        public abstract UniTask Load(CancellationToken cancellationToken);
        public abstract void Install(IContainerBuilder builder);
    }
}