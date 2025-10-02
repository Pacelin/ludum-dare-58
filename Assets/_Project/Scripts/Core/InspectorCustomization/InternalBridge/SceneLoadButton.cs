using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Core.InspectorCustomization.InternalBridge
{
    [ToolbarElement(EToolbarPosition.RightLeftAlign)]
    internal sealed class SceneLoadButton : ToolbarButton
    {
        public SceneLoadButton()
        {
            style.paddingLeft = 4;
            style.paddingRight = 4;
            text = "Load";
            iconImage = Background.FromTexture2D((Texture2D)EditorGUIUtility.IconContent("icons/d_BuildSettings.SelectedIcon.png").image);
            
            clicked += OnClick;
        }

        ~SceneLoadButton()
        {
            clicked -= OnClick;
        }

        private void OnClick()
        {
            var menu = new GenericMenu();
            var scenePaths = AssetDatabase.FindAssets("t:SceneAsset")
                .Select(guid => AssetDatabase.GUIDToAssetPath((string)guid));
            foreach (var path in scenePaths)
            {
                if (path.StartsWith("Assets"))
                {
                    var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                    menu.AddItem(new GUIContent(asset.name), false, () => EditorSceneManager.OpenScene(path));
                }
            }
            
            menu.ShowAsContext();
        }
    }
}