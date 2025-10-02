using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Settings;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Scripts.Core.Lifetime
{
    public static class ApplicationEntryPoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
#if UNITY_EDITOR
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
                return;
#endif
            
            UniTask.Void(async cancellationToken =>
            {
                try
                {
                    await Addressables.InitializeAsync();
                    if (LocalizationSettings.InitializationOperation.IsValid())
                        await LocalizationSettings.InitializationOperation;
                    DOTween.Init(logBehaviour: LogBehaviour.Verbose);

                    var applicationSettings = await Addressables
                        .LoadAssetAsync<ApplicationSettings>(ApplicationSettings.ADDRESS)
                        .ToUniTask(cancellationToken: cancellationToken, autoReleaseWhenCanceled: true);
                    cancellationToken.ThrowIfCancellationRequested();

                    var eventSystem = Object.Instantiate(applicationSettings.EventSystemPrefab).gameObject;
                    Object.DontDestroyOnLoad(eventSystem);
                    eventSystem.name = "[EVENT SYSTEM]";
                    
                    foreach (var installer in applicationSettings.Installers)
                    {
                        await installer.Load(cancellationToken);
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    
                    CreateApplicationScope(applicationSettings.Installers, cancellationToken);
                    Addressables.Release(applicationSettings);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }, ApplicationState.ExitCancellationToken);
        }

        private static void CreateApplicationScope(ApplicationInstaller[] applicationInstallers, CancellationToken cancellationToken)
        {
            var gameObject = new GameObject("[APPLICATION]");
            Object.DontDestroyOnLoad(gameObject);
            gameObject.SetActive(false);
            
            var monoProvider = gameObject.AddComponent<ApplicationMonoProvider>();
            ApplicationState.Initialize(monoProvider);
            
            var applicationScope = gameObject.AddComponent<ApplicationLifetimeScope>();
            applicationScope.Construct(applicationInstallers);
            applicationScope.autoRun = false;

            gameObject.SetActive(true);
            applicationScope.Build();
            
            var scopeParent = LifetimeScope.EnqueueParent(applicationScope);
            cancellationToken.Register(() => scopeParent.Dispose());
        }
    }
}