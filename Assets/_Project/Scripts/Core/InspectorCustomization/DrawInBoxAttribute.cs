using System;
using System.Diagnostics;
using UnityEngine;

namespace Scripts.Core.InspectorCustomization
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field)]
    public class DrawInBoxAttribute : PropertyAttribute
    {
        public bool ShowLabel { get; set; }
    }
}