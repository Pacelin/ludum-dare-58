using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scripts.Core.SceneManagement
{
    public abstract class SceneManageHandler : MonoBehaviour
    {
        public virtual UniTask BeforeLoad(CancellationToken cancellationToken) => UniTask.CompletedTask;
        public virtual UniTask AfterLoad(CancellationToken cancellationToken) => UniTask.CompletedTask;
    }
}