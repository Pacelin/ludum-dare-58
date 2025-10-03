using System;
using Cysharp.Threading.Tasks;
using Scripts.Core.Lifetime;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Scripts.Core.SceneManagement
{
    public static class SceneManager
    {
        public static ScenesData Database => _scenesData;
        
        private static ScenesData _scenesData;
        
        internal static void Initialize(ScenesData scenesData) => _scenesData = scenesData;

        public static async UniTask LoadScene(AssetReference scene, 
            SceneManageHandler handlerPrefab,
            Action<IContainerBuilder> extraParams)
        {  
            var handler = Object.Instantiate(handlerPrefab);
            Object.DontDestroyOnLoad(handler);
            
            await handler.BeforeLoad(ApplicationState.ExitCancellationToken);
            await LoadScene(scene, extraParams);
            await handler.AfterLoad(ApplicationState.ExitCancellationToken);

            Object.Destroy(handler);
        }
        
        public static async UniTask LoadScene(AssetReference scene, 
            SceneManageHandler handlerPrefab)
        {
            var handler = Object.Instantiate(handlerPrefab);
            Object.DontDestroyOnLoad(handler);
            
            await handler.BeforeLoad(ApplicationState.ExitCancellationToken);
            await LoadScene(scene);
            await handler.AfterLoad(ApplicationState.ExitCancellationToken);
            
            Object.Destroy(handler);
        }
        
        public static async UniTask LoadScene(AssetReference scene,
            Action<IContainerBuilder> extraParams)
        {
            using (LifetimeScope.Enqueue(extraParams))
            {
                await LoadScene(scene).SuppressCancellationThrow();
            }
        }

        public static async UniTask LoadScene(AssetReference scene)
        {
            await Addressables.LoadSceneAsync(scene)
                .ToUniTask(cancellationToken: ApplicationState.ExitCancellationToken, autoReleaseWhenCanceled: true);
            if (ApplicationState.ExitCancellationToken.IsCancellationRequested)
                return;
            ApplicationState.SetPause(false);
        }
    }
}