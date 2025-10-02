using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Scripts.Core.InspectorCustomization.InternalBridge
{
    [InitializeOnLoad]
    internal static class PlayModeProcessor
    {
        private static bool RunWithFirstScene
        {
            get => EditorPrefs.GetBool("run_with_first_scene");
            set => EditorPrefs.SetBool("run_with_first_scene", value);
        }
        
        private static string LastActiveScenePath
        {
            get => EditorPrefs.GetString("last_active_scene_toolbar");
            set => EditorPrefs.SetString("last_active_scene_toolbar", value);
        }
        
        static PlayModeProcessor()
        {
            PlayModeButtons.onPlayModeButtonsCreated += OnPlayModeButtonsCreated;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeButtonsCreated(VisualElement root)
        {
            var playToggle = root.Q<EditorToolbarToggle>("Play");
            playToggle.RegisterCallback<MouseDownEvent>((evt =>
            {
                if (evt.button != 1)
                    return;
                GenericMenu genericMenu = new GenericMenu();
                genericMenu.AddItem(new GUIContent("Run with first scene"), RunWithFirstScene, () => RunWithFirstScene = !RunWithFirstScene);
                genericMenu.ShowAsContext();
            }));
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            if (!RunWithFirstScene)
                return;
            if (obj == PlayModeStateChange.ExitingEditMode)
            {
                LastActiveScenePath = SceneManager.GetActiveScene().path;
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(0));
            }
            else if (obj == PlayModeStateChange.EnteredEditMode)
                EditorSceneManager.OpenScene(LastActiveScenePath);
        }
    }
}