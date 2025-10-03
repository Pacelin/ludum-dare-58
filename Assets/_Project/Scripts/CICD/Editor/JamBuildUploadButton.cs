using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Scripts.Core.InspectorCustomization.InternalBridge;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

namespace TSS.Jams.Editor
{
    [UsedImplicitly]
    [ToolbarElement(EToolbarPosition.LeftLeftAlign)]
    public class JamBuildUploadButton : ToolbarButton
    {
        private const string BUTLER_PATH = @"c:\itch-butler\butler.exe";
        private const string UPGRADE_ARGS = "upgrade --assume-yes";
        private const string LOGIN_ARGS = "login";
        private const string PUSH_ARGS = "push \"{0}\" {1}/{2}:{3} --userversion {4}";
        
        public JamBuildUploadButton()
        {
            text = "Upload Build (Itch)";
            clicked += RunUpload;
            RegisterCallback<MouseDownEvent>(OnMouseDown);
        }

        ~JamBuildUploadButton()
        {
            clicked -= RunUpload;
        }
        
        private void OnMouseDown(MouseDownEvent evt)
        {
            if (evt.button != 1)
                return;
            EditorWindow.GetWindow<JamBuildUploadSettingsWindow>(true).Initialize();
        }
        
        private async void RunUpload()
        {
            if (Progress.Exists(JamBuildUploadSettings.LastProgressId))
            {
                Debug.Log("<color=#ff0000>Operation already executed.</color>");
                return;
            }
            if (!Directory.Exists(JamBuildUploadSettings.LastBuildPath))
            {
                Debug.Log("<color=#ff0000>Last build directory not found.</color>");
                return;
            }

            var directory = JamBuildUploadSettings.LastBuildPath;
            var user = JamBuildUploadSettings.UserName.ToLower();
            var game = JamBuildUploadSettings.GameName.ToLower();
            var version = JamBuildUploadSettings.Version;
            var channel = JamBuildUploadSettings.Platform;
            var progressId = Progress.Start("Upload Build (Itch)");
            JamBuildUploadSettings.LastProgressId = progressId;
            await Task.Run(() =>
            {
                try
                {
                    CheckForUpdates(progressId);
                    Login(progressId);
                    Upload(progressId, directory, user, game, channel, version);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    Progress.Finish(progressId, Progress.Status.Failed);
                }
            });
            Progress.Finish(progressId);
        }

        private void CheckForUpdates(int progressId)
        {
            Progress.SetDescription(progressId, "Check for butler updates");
            Progress.Report(progressId, 0f);
            var startInfo = new ProcessStartInfo(BUTLER_PATH, UPGRADE_ARGS);
            startInfo.UseShellExecute = false;
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            Progress.Report(progressId, 0.1f);
        }

        private void Login(int progressId)
        {
            Progress.SetDescription(progressId, "Login butler");
            var startInfo = new ProcessStartInfo(BUTLER_PATH, LOGIN_ARGS);
            startInfo.UseShellExecute = false;
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            Progress.Report(progressId, 0.3f);
        }

        private void Upload(int progressId, string directory, string user, string game, string channel, string version)
        {
            Progress.SetDescription(progressId, "Upload");

            var arguments = string.Format(PUSH_ARGS, directory, user, game, channel, version);
            var startInfo = new ProcessStartInfo(BUTLER_PATH, arguments);
            
            startInfo.UseShellExecute = false;
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            Progress.Report(progressId, 1f);
        }
    }
}