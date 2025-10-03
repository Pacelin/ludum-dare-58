using UnityEditor;
using UnityEngine;

namespace TSS.Jams.Editor
{
    [EditorWindowTitle(title = "Itch Upload Settings")]
    public class JamBuildUploadSettingsWindow : EditorWindow
    {
        private string _user;
        private string _game;
        private string _version;
        private string _platform;

        public void Initialize()
        {
            _user = JamBuildUploadSettings.UserName;
            _game = JamBuildUploadSettings.GameName;
            _version = JamBuildUploadSettings.Version;
            _platform = JamBuildUploadSettings.Platform;
            position = new(Event.current.mousePosition + new Vector2(-150, 100), 
                new Vector2(300, 105));
        }

        private void OnGUI()
        {
            _user = EditorGUILayout.TextField("User codename: ", _user);
            _game = EditorGUILayout.TextField("Game codename: ", _game);
            _platform = EditorGUILayout.TextField("Platform codename: ", _platform);
            _version = EditorGUILayout.TextField("Version: ", _version);

            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button(new GUIContent("Open Page")))
                Application.OpenURL($"https://{_user.ToLower()}.itch.io/{_game.ToLower()}");
            
            if (GUILayout.Button(new GUIContent("Save")))
            {
                JamBuildUploadSettings.UserName = _user;
                JamBuildUploadSettings.GameName = _game;
                JamBuildUploadSettings.Version = _version;
                Close();
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }
}