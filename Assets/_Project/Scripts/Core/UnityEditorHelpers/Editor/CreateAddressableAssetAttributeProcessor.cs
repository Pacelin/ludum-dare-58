using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Scripts.Core.UnityEditorHelpers.Editor
{
    public static class CreateAddressableAssetAttributeProcessor
    {
        [OnAssetsImport]
        private static void Process()
        {
            var types = TypeCache.GetTypesWithAttribute<CreateAddressableAssetAttribute>();
            foreach (var type in types)
            {
                if (!typeof(ScriptableObject).IsAssignableFrom(type))
                    return;
                var attribute = type.GetCustomAttribute<CreateAddressableAssetAttribute>();
                var path = CreateAddressableAssetAttribute.PATH + attribute.Name + ".asset";
                AssetsHelper.ValidateAsset(type, path);
                AssetsHelper.ValidateAddressable(path, attribute.Address, CreateAddressableAssetAttribute.GROUP);
            }
        }
    }
}