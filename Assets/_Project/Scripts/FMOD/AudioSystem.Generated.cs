// Auto-generated code. Reference: "Packages/com.tss.cms/Editor/CMSGenerator.cs"

// ReSharper disable RedundantUsingDirective
#pragma warning disable CS1998

using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using FMODUnity;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts.Audio
{
    public static class AudioSystem
    {
	    public static class Volumes
	    {
		    public static float MasterVolume
		    {
			    get
			    {
				    _masterBus.getVolume(out var volume);
				    return volume;
			    }
			    set
			    {
				    _masterBus.setVolume(value);
				    PlayerPrefs.SetFloat("master_volume", value);
			    }
		    }

		    public static float GetVolume(int index)
		    {
			    _buses[index].getVolume(out var volume);
			    return volume;
		    }

		    public static void SetVolume(int index, float volume)
		    {
			    _buses[index].setVolume(volume);
			    _buses[index].getID(out var id);
			    PlayerPrefs.SetFloat("volume_of_" + id, volume);
		    }
	    }
	    
		public static class Global
		{
		}
    
		public static SoundEvent_UI_Down UI_Down { get; } = new();
		public static SoundEvent_UI_Hover UI_Hover { get; } = new();
		public static SoundEvent_UI_Slider UI_Slider { get; } = new();
		public static SoundEvent_UI_PopupAppear UI_PopupAppear { get; } = new();
		
		private static FMOD.Studio.Bus _masterBus;
		private static FMOD.Studio.Bus[] _buses;

        public static async UniTask Initialize(AudioVolumes volumes, CancellationToken cancellationToken)
        {
			RuntimeManager.LoadBank("Master.strings", true);
			RuntimeManager.LoadBank("Master", true);

            await UniTask.WaitUntil(() => FMODUnity.RuntimeManager.HaveAllBanksLoaded);
            await UniTask.WaitWhile(FMODUnity.RuntimeManager.AnySampleDataLoading);
            
            _masterBus = FMODUnity.RuntimeManager.GetBus(volumes.MasterBusPath);
            _masterBus.setVolume(PlayerPrefs.GetFloat("master_volume", volumes.DefaultMasterVolume));

            _buses = new FMOD.Studio.Bus[volumes.BusesPaths.Length];
            for (int i = 0; i < _buses.Length; i++)
            {
	            _buses[i] = FMODUnity.RuntimeManager.GetBus(volumes.BusesPaths[i]);
	            _buses[i].getID(out var busId);
	            _buses[i].setVolume(PlayerPrefs.GetFloat("volume_of_" + busId, volumes.DefaultVolume));
            }
        }
    }

	public class SoundEvent_UI_Down : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 551;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 1937718590, Data2 = 1153062528, Data3 = 1319071167, Data4 = -621755114 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_UI_Hover : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 551;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 790788430, Data2 = 1137934485, Data3 = 1109494915, Data4 = -1676451506 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_UI_Slider : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 994;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 1669917026, Data2 = 1124881252, Data3 = 534556034, Data4 = -161086704 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_UI_PopupAppear : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 416;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -2090072161, Data2 = 1288527112, Data3 = 328839071, Data4 = 2067559421 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

}