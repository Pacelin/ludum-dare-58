using System;
using System.Diagnostics;

namespace Scripts.Core.InspectorCustomization
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Class)]
    public class InterfaceSelectNameAttribute : Attribute
    {
        public string Name { get; }
        
        public InterfaceSelectNameAttribute(string name) => 
            Name = name;
    }
}