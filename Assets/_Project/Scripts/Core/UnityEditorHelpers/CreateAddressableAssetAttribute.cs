using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Scripts.Core.UnityEditorHelpers
{
    [AttributeUsage(AttributeTargets.Class)]
    [MeansImplicitUse]
    [Conditional("UNITY_EDITOR")]
    public class CreateAddressableAssetAttribute : Attribute
    {
        public static string GROUP = "Generated Assets";
        public static string PATH = "Assets/_Project/Settings/";
        
        public string Name { get; }
        public string Address { get; }
        
        public CreateAddressableAssetAttribute(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }
}