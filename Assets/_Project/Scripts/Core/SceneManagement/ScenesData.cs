using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts.Core.SceneManagement
{
    [System.Serializable]
    public class ScenesData
    {
        public AssetReference EnterNickname => _enterNickname;
        public AssetReference MainMenu => _mainMenu;
        public AssetReference Game => _game;

        [SerializeField] private AssetReference _enterNickname;
        [SerializeField] private AssetReference _mainMenu;
        [SerializeField] private AssetReference _game;
    }
}