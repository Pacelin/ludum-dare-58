using System.Globalization;
using System.Linq;
using FMODUnity;
using Scriban;
using Scripts.Core.UnityEditorHelpers.Editor;
using UnityEditor;
using UnityEngine;

namespace TSS.Audio.Editor
{
    internal class GenerationData
    {
        public string[] banks;
        public EventData[] events;
        public ParameterData[] parameters;
        
        public bool tss_core;
    }

    internal class EventData
    {
        public int guid_1;
        public int guid_2;
        public int guid_3;
        public int guid_4;
        
        public string name;
        public string is_one_shot_str;
        public string length_str;
        
        public ParameterData[] parameters;
    }

    internal class ParameterData
    {
        public uint id_1;
        public uint id_2;
        public string name;
        
        public bool is_labeled;
        public string[] labels;

        public bool is_discrete;

        public string min_str;
        public string max_str;
    }
    
    internal static class FMODAudioGenerator
    {
        private const string TEMPLATE_PATH = "Assets/_Project/Scripts/FMOD/Editor/fmod_template.txt";
        private const string GENERATION_PATH = "Assets/_Project/Scripts/FMOD/AudioSystem.Generated.cs";
        
        public static void Generate()
        {
            AssetDatabase.StartAssetEditing();
            try
            {
                var templateAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(TEMPLATE_PATH);
                var template = Template.Parse(templateAsset.text);
                var render = template.Render(CollectData());
                AssetsHelper.WriteFile(GENERATION_PATH, render);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.Refresh();
            }
        }

        private static GenerationData CollectData()
        {
            var banks = EventManager.Banks.Select(bank => bank.Name).ToArray();
            var events = EventManager.Events.Select(evt =>
            {
                return new EventData()
                {
                    guid_1 = evt.Guid.Data1,
                    guid_2 = evt.Guid.Data2,
                    guid_3 = evt.Guid.Data3,
                    guid_4 = evt.Guid.Data4,
                    name = FMODUtilsInternal.GetEventName(evt),
                    is_one_shot_str = evt.IsOneShot.ToString().ToLower(),
                    length_str = evt.Length.ToString(),
                    parameters = evt.LocalParameters.Select(par =>
                    {
                        return new ParameterData()
                        {
                            id_1 = par.ID.data1,
                            id_2 = par.ID.data2,
                            is_discrete = par.Type == ParameterType.Discrete,
                            is_labeled = par.Type == ParameterType.Labeled,
                            labels = par.Labels,
                            min_str = par.Type == ParameterType.Discrete ? par.Min.ToString("0") :
                                par.Min.ToString(CultureInfo.InvariantCulture),
                            max_str = par.Type == ParameterType.Discrete ? par.Max.ToString("0") :
                                par.Max.ToString(CultureInfo.InvariantCulture),
                            name = FMODUtilsInternal.GetParameterName(par)
                        };
                    }).ToArray()
                };
            }).ToArray();
            var parameters = EventManager.Parameters.Where(p => p.IsGlobal).Select(par =>
            {
                return new ParameterData()
                {
                    id_1 = par.ID.data1,
                    id_2 = par.ID.data2,
                    is_discrete = par.Type == ParameterType.Discrete,
                    is_labeled = par.Type == ParameterType.Labeled,
                    labels = par.Labels,
                    min_str = par.Type == ParameterType.Discrete ? par.Min.ToString("0") :
                        par.Min.ToString(CultureInfo.InvariantCulture),
                    max_str = par.Type == ParameterType.Discrete ? par.Max.ToString("0") :
                        par.Max.ToString(CultureInfo.InvariantCulture),
                    name = FMODUtilsInternal.GetParameterName(par)
                };
            }).ToArray();
            return new GenerationData()
            {
                banks = banks,
                events = events,
                parameters = parameters,
#if TSS_CORE
                tss_core = true
#else
                tss_core = false
#endif
            };
        }
    }
}