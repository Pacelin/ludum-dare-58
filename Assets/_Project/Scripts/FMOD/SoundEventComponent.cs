using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace TSS.Audio
{
    public class SoundEventComponent : MonoBehaviour, ISoundEvent
    {
        public bool IsOneShot
        {
            get
            {
                if (!_hasEventDescription)
                {
                    _eventDescription = RuntimeManager.GetEventDescription(_eventRef);
                    _hasEventDescription = true;
                }

                _eventDescription.isOneshot(out var oneshot);
                return oneshot;
            }
        }

        public float Length
        {
            get
            {
                if (!_hasEventDescription)
                {
                    _eventDescription = RuntimeManager.GetEventDescription(_eventRef);
                    _hasEventDescription = true;
                }

                _eventDescription.getLength(out var length);
                return length;
            }
        } 

        [SerializeField] private EventReference _eventRef;

        private bool _hasEventDescription;
        private EventDescription _eventDescription;
        
        public void PlayOneShot() => RuntimeManager.PlayOneShot(_eventRef.Guid);
        public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_eventRef.Guid, attachTo);
        public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_eventRef.Guid, point);

        public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_eventRef.Guid));
        ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

        public class Instance : SoundEventInstance
        {
            public Instance(EventInstance eventInstance) : base(eventInstance) { }
        }
    }
}