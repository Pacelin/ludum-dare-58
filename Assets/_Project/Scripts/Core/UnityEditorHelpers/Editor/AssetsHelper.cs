using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Scripts.Core.UnityEditorHelpers.Editor
{
    [PublicAPI]
    public static class AssetsHelper
    {
        public static bool AssetPathExists(string path) =>
            AssetDatabase.GetAllAssetPaths().Contains(path);
        
        public static void WriteFile(string filePath, string content)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            ValidateDirectoryExists(directoryPath);
            File.WriteAllText(filePath, content);
        }
        
        public static void ValidateAsset(Type assetType, string path)
        {
            if (AssetPathExists(path)) 
                return;
            
            var directoryPath = Path.GetDirectoryName(path);
            ValidateDirectoryExists(directoryPath);
            var newAsset = ScriptableObject.CreateInstance(assetType);
            AssetDatabase.CreateAsset(newAsset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void ValidateDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        
        public static void ValidateAddressableGroup(string groupName)
        {
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                Debug.LogError("AddressableAssetSettings not exists");
                return;
            }
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var group = settings.FindGroup(groupName);
            if (!group)
            {
                settings.CreateGroup(groupName, false, false, false,
                    settings.DefaultGroup.Schemas);
                EditorUtility.SetDirty(settings);
            }
        }
        
        public static void ValidateAddressable(string path, string address, string customGroup = null)
        {
            if (!AddressableAssetSettingsDefaultObject.SettingsExists)
            {
                Debug.LogError("AddressableAssetSettings not exists");
                return;
            }
            bool isDirty = false;
            
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var group = settings.DefaultGroup;
            if (customGroup != null)
            {
                ValidateAddressableGroup(customGroup);
                group = settings.FindGroup(customGroup);
            }
            
            var assetGUID = AssetDatabase.AssetPathToGUID(path);
            var entry = settings.FindAssetEntry(assetGUID);
            if (entry == null || entry.parentGroup != group)
            {
                entry = settings.CreateOrMoveEntry(assetGUID, group, false, false);
                isDirty = true;
            }

            if (entry.address != address)
            {
                entry.address = address;
                isDirty = true;
            }
            
            if (isDirty)
                EditorUtility.SetDirty(settings);
        }
    }
}