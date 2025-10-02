using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Scripts.Core.InspectorCustomization.InternalBridge
{
    [AttributeUsage(AttributeTargets.Class)]
    [MeansImplicitUse]
    [Conditional("UNITY_EDITOR")]
    public class ToolbarElementAttribute : Attribute
    {
        public EToolbarPosition Position { get; }
        public int Order { get; set; }
        
        public ToolbarElementAttribute(EToolbarPosition position) => Position = position;
    }
}