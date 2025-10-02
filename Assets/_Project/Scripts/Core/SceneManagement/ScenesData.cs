using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts.Core.SceneManagement
{
    [System.Serializable]
    public class ScenesData
    {
        public AssetReference MainMenu => _mainMenu;

        [SerializeField] private AssetReference _mainMenu;
    }
}