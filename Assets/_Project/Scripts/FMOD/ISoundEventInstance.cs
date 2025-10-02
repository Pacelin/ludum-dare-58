using UnityEngine;

namespace TSS.Audio
{
    public interface ISoundEventInstance
    {
        bool IsValid();
        void Release();
        void Start();
        void Stop(bool allowFadeOut);
        
        ESoundState GetPlaybackState();
        
        void SetVolume(float volume);
        float GetVolume();
        
        void SetPitch(float pitch);
        float GetPitch();

        void SetPaused(bool paused);
        bool GetPaused();

        void SetReverbLevel(int index, float reverb);
        float GetReverbLevel(int index);

        void SetTimelinePosition(int position);
        int GetTimelinePosition();

        void SetWorldPosition(Vector3 position);
        
        public void AttachTo(GameObject go);
        public void AttachTo(GameObject go, Rigidbody rigidbody);
        public void AttachTo(GameObject go, Rigidbody2D rigidbody2D);
        public void Detach();
    }
}