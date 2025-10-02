using System;
using JetBrains.Annotations;

namespace Scripts.Core.UnityEditorHelpers.Editor
{
    [AttributeUsage(AttributeTargets.Method)]
    [MeansImplicitUse]
    [PublicAPI]
    public class OnAssetsImportAttribute : Attribute
    {
    }
}