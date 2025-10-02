using UnityEngine;

namespace TSS.Audio
{
    public class PlaySoundEvent : MonoBehaviour
    {
        [SerializeField] private SoundEvent _event;

        private SoundEventInstance _instance;
        private void OnEnable()
        {
            _instance = _event.CreateInstance();
            _instance.Start();
        }

        private void OnDisable()
        {
            _instance.Stop(true);
            _instance.Release();
        }
    }
}