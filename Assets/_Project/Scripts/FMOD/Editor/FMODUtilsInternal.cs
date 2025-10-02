using System.Text;
using FMODUnity;
using UnityEditor;
using UnityEngine;

namespace TSS.Audio.Editor
{
    internal static class FMODUtilsInternal
    {
        private const string ICON_PATH = "Assets/Plugins/FMOD/images/StudioIcon.png";

        public static Texture2D GetFMODStudioIcon() =>
            AssetDatabase.LoadAssetAtPath<Texture2D>(ICON_PATH);

        public static string GetParameterName(EditorParamRef paramRef)
        {
            var builder = new StringBuilder();
            const string separators = "_- ";
            bool lastIsSeparator = true;
            
            foreach (var ch in paramRef.Name)
            {
                if (separators.Contains(ch))
                {
                    lastIsSeparator = true;
                }
                else if (lastIsSeparator)
                {
                    lastIsSeparator = false;
                    builder.Append(char.ToUpper(ch));
                }
                else
                {
                    builder.Append(ch);
                }
            }

            return builder.ToString().Replace("/", "_");
        }
        
        public static string GetEventName(EditorEventRef eventRef)
        {
            var withoutPrefix = eventRef.Path.Replace("event:/", "").Replace("snapshot:/", "");
            var builder = new StringBuilder();
            const string separators = "_- ";
            bool lastIsSeparator = true;
            
            foreach (var ch in withoutPrefix)
            {
                if (separators.Contains(ch))
                {
                    lastIsSeparator = true;
                }
                else if (lastIsSeparator)
                {
                    lastIsSeparator = false;
                    builder.Append(char.ToUpper(ch));
                }
                else
                {
                    builder.Append(ch);
                }
            }

            return builder.ToString().Replace("/", "_");
        }
    }
}