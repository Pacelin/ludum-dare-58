using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Scripts.Core.UnityEditorHelpers.Editor
{
    public static class CreateAssetAttributeProcessor
    {
        [OnAssetsImport]
        private static void Process()
        {
            var types = TypeCache.GetTypesWithAttribute<CreateAssetAttribute>();
            foreach (var type in types)
            {
                if (!typeof(ScriptableObject).IsAssignableFrom(type))
                    return;
                var attribute = type.GetCustomAttribute<CreateAssetAttribute>();
                var path = CreateAssetAttribute.PATH + attribute.Name + ".asset";
                AssetsHelper.ValidateAsset(type, path);
            }
        }
    }
}