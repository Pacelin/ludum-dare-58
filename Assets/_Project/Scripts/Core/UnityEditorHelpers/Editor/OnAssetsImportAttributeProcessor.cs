using UnityEditor;

namespace Scripts.Core.UnityEditorHelpers.Editor
{
    public class OnAssetsImportAttributeProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets, 
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var methods = TypeCache.GetMethodsWithAttribute<OnAssetsImportAttribute>();
            foreach (var method in methods)
            {
                if (!method.IsStatic)
                    continue;
                method.Invoke(null, null);
            }
        }
    }
}