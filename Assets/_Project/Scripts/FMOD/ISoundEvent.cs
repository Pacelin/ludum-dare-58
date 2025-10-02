using UnityEngine;

namespace TSS.Audio
{
    public interface ISoundEvent
    {
        bool IsOneShot { get; }
        float Length { get; }
        void PlayOneShot();
        void PlayOneShotAttached(GameObject attachTo);
        void PlayOneShotInPoint(Vector3 point);
        
        ISoundEventInstance CreateInstance();
    }
}