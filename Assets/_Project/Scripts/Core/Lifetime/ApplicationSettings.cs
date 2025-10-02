using Scripts.Core.UnityEditorHelpers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Core.Lifetime
{
    [CreateAddressableAsset("SO_ApplicationSettings", ADDRESS)]
    internal class ApplicationSettings : ScriptableObject
    {
        public const string ADDRESS = "Application Settings";
        
        public EventSystem EventSystemPrefab => _eventSystemPrefab;
        public ApplicationInstaller[] Installers => _installers;

        [SerializeField] private EventSystem _eventSystemPrefab;
        [SerializeField] private ApplicationInstaller[] _installers;
    }
}