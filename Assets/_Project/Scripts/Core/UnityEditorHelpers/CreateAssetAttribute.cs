using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Scripts.Core.UnityEditorHelpers
{
    [AttributeUsage(AttributeTargets.Class)]
    [MeansImplicitUse]
    [Conditional("UNITY_EDITOR")]
    public class CreateAssetAttribute : Attribute
    {
        public static string PATH = "Assets/_Project/Settings/";
        
        public string Name { get; }
        
        public CreateAssetAttribute(string name)
        {
            Name = name;
        }
    }
}