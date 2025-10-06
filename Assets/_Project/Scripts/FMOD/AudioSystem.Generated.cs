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
				    UserDataManager.SetFloat("master_volume", value);
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
			    UserDataManager.SetFloat("volume_of_" + id, volume);
		    }
	    }
	    
		public static class Global
		{
		}
    
		public static SoundEvent_UI_Down UI_Down { get; } = new();
		public static SoundEvent_UI_Hover UI_Hover { get; } = new();
		public static SoundEvent_UI_Slider UI_Slider { get; } = new();
		public static SoundEvent_UI_PopupAppear UI_PopupAppear { get; } = new();
		public static SoundEvent_Game_MainMenuMusic Game_MainMenuMusic { get; } = new();
		public static SoundEvent_Game_PauseMusicNight Game_PauseMusicNight { get; } = new();
		public static SoundEvent_Game_GameMusic Game_GameMusic { get; } = new();
		public static SoundEvent_Game_Plant Game_Plant { get; } = new();
		public static SoundEvent_Game_PauseMusicDay Game_PauseMusicDay { get; } = new();
		public static SoundEvent_Game_Dig Game_Dig { get; } = new();
		public static SoundEvent_Journal_Tab Journal_Tab { get; } = new();
		public static SoundEvent_Journal_TabHover Journal_TabHover { get; } = new();
		public static SoundEvent_Journal_Open Journal_Open { get; } = new();
		public static SoundEvent_Journal_NewNote Journal_NewNote { get; } = new();
		public static SoundEvent_Game_UnlockFlower Game_UnlockFlower { get; } = new();
		public static SoundEvent_Game_Butterfly Game_Butterfly { get; } = new();
		public static SoundEvent_Game_Catch Game_Catch { get; } = new();
		
		private static FMOD.Studio.Bus _masterBus;
		private static FMOD.Studio.Bus[] _buses;

        public static async UniTask Initialize(AudioVolumes volumes, CancellationToken cancellationToken)
        {
			RuntimeManager.LoadBank("Master.strings", true);
			RuntimeManager.LoadBank("Master", true);

            await UniTask.WaitUntil(() => FMODUnity.RuntimeManager.HaveAllBanksLoaded);
            await UniTask.WaitWhile(FMODUnity.RuntimeManager.AnySampleDataLoading);
            
            _masterBus = FMODUnity.RuntimeManager.GetBus(volumes.MasterBusPath);
            _masterBus.setVolume(UserDataManager.GetFloat("master_volume", volumes.DefaultMasterVolume));

            _buses = new FMOD.Studio.Bus[volumes.BusesPaths.Length];
            for (int i = 0; i < _buses.Length; i++)
            {
	            _buses[i] = FMODUnity.RuntimeManager.GetBus(volumes.BusesPaths[i]);
	            _buses[i].getID(out var busId);
	            _buses[i].setVolume(UserDataManager.GetFloat("volume_of_" + busId, volumes.DefaultVolume));
            }
        }
    }

	public class SoundEvent_UI_Down : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 100;

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
		public float Length => 115;

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
		public float Length => 400;

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
		public float Length => 0;

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

	public class SoundEvent_Game_MainMenuMusic : ISoundEvent
	{
		public bool IsOneShot => false;
		public float Length => 122880;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 946637591, Data2 = 1106365952, Data3 = 771093915, Data4 = 1022867313 };

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

	public class SoundEvent_Game_PauseMusicNight : ISoundEvent
	{
		public bool IsOneShot => false;
		public float Length => 122880;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -500334208, Data2 = 1180539227, Data3 = -437109599, Data4 = -894772982 };

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

	public class SoundEvent_Game_GameMusic : ISoundEvent
	{
		public bool IsOneShot => false;
		public float Length => 245760;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 704146316, Data2 = 1080214396, Data3 = -581027137, Data4 = -1337645722 };

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

	public class SoundEvent_Game_Plant : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 450;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 1856748530, Data2 = 1170620831, Data3 = 1223083657, Data4 = -810326411 };

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

	public class SoundEvent_Game_PauseMusicDay : ISoundEvent
	{
		public bool IsOneShot => false;
		public float Length => 122880;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 1064517880, Data2 = 1127640710, Data3 = -815622762, Data4 = -626429070 };

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

	public class SoundEvent_Game_Dig : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 450;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 223975893, Data2 = 1116323776, Data3 = -230642505, Data4 = 82586545 };

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

	public class SoundEvent_Journal_Tab : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 250;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -2145088236, Data2 = 1284667434, Data3 = 623237291, Data4 = -1118006707 };

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

	public class SoundEvent_Journal_TabHover : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 95;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -411965667, Data2 = 1163783608, Data3 = 773201809, Data4 = -630037517 };

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

	public class SoundEvent_Journal_Open : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 340;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -925599243, Data2 = 1226781010, Data3 = -549793380, Data4 = 2141855515 };

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

	public class SoundEvent_Journal_NewNote : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 950;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 1442618387, Data2 = 1324161679, Data3 = 219835014, Data4 = 87786711 };

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

	public class SoundEvent_Game_UnlockFlower : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 2195;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -1075404189, Data2 = 1303903864, Data3 = -1554176615, Data4 = -605393814 };

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

	public class SoundEvent_Game_Butterfly : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 5000;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 1517831283, Data2 = 1136543127, Data3 = 965832580, Data4 = -889852680 };

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

	public class SoundEvent_Game_Catch : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 449;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 233737477, Data2 = 1132038974, Data3 = -1234253905, Data4 = 1002245338 };

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