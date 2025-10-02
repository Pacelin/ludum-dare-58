using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace TSS.Audio
{
    public class SoundEventInstance : ISoundEventInstance
    {
        protected EventInstance Instance => _instance;
        
        private EventInstance _instance;
        
        protected SoundEventInstance(EventInstance instance) =>
            _instance = instance;

        public bool IsValid() => _instance.isValid();
        public void Release() => _instance.release();

        public void Start() => _instance.start();
        public void Stop(bool allowFadeOut) => _instance.stop(allowFadeOut ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE);

        public ESoundState GetPlaybackState()
        {
            _instance.getPlaybackState(out var state);
            return state == PLAYBACK_STATE.STOPPED ? ESoundState.Stopped :
                state == PLAYBACK_STATE.SUSTAINING ? ESoundState.Paused : ESoundState.Playing;
        }
        
        public void SetVolume(float volume) => _instance.setVolume(volume);
        public float GetVolume()
        {
            _instance.getVolume(out var volume);
            return volume;
        }

        public void SetPitch(float pitch) => _instance.setPitch(pitch);
        public float GetPitch()
        {
            _instance.getPitch(out var pitch);
            return pitch;
        }

        public void SetPaused(bool paused) => _instance.setPaused(paused);
        public bool GetPaused()
        {
            _instance.getPaused(out var paused);
            return paused;
        }

        public void SetReverbLevel(int index, float reverb) => _instance.setReverbLevel(index, reverb);
        public float GetReverbLevel(int index)
        {
            _instance.getReverbLevel(index, out var reverb);
            return reverb;
        }

        public void SetTimelinePosition(int position) => _instance.setTimelinePosition(position);
        public int GetTimelinePosition()
        {
            _instance.getTimelinePosition(out var position);
            return position;
        }

        public void SetWorldPosition(Vector3 position) => _instance.set3DAttributes(position.To3DAttributes());
        
        public void AttachTo(GameObject go) => RuntimeManager.AttachInstanceToGameObject(_instance, go);
        public void AttachTo(GameObject go, Rigidbody rigidbody) => RuntimeManager.AttachInstanceToGameObject(_instance, go, rigidbody);
        public void AttachTo(GameObject go, Rigidbody2D rigidbody2D) => RuntimeManager.AttachInstanceToGameObject(_instance, go, rigidbody2D);
        public void Detach() => RuntimeManager.DetachInstanceFromGameObject(_instance);
    }
}