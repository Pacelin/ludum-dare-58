using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Scripts.Core.SceneManagement;
using UnityEngine;
using VContainer.Unity;

namespace Scripts.Loading
{
    [UsedImplicitly]
    public class ApplicationLoading : IInitializable
    {
        public void Initialize()
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("username", null)))
                SceneManager.LoadScene(SceneManager.Database.EnterNickname).Forget();
            else
                SceneManager.LoadScene(SceneManager.Database.MainMenu).Forget();
        }
    }
}