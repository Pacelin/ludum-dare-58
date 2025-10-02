using UnityEngine;

namespace Scripts.Audio
{
    [System.Serializable]
    public class AudioVolumes
    {
        public string MasterBusPath => _masterBusPath;
        public string[] BusesPaths => _busesPaths;
        
        public float DefaultMasterVolume => _defaultMasterVolume;
        public float DefaultVolume => _defaultVolume;
        
        [SerializeField] private string _masterBusPath;
        [SerializeField] private string[] _busesPaths;
        [SerializeField] private float _defaultMasterVolume;
        [SerializeField] private float _defaultVolume;
    }
}