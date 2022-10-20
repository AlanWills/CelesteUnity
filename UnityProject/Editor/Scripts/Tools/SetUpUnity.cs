using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace CelesteEditor.UnityProject
{
    public static class SetUpUnity
    {
        #region Menu Item

        [MenuItem("Celeste/Bootstrap/1) Set Up Unity", priority = 1)]
        public static void ExecuteSetUpUnity()
        {
            // Change .Net Framework
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_4_6);
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone, ApiCompatibilityLevel.NET_4_6);
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WebGL, ApiCompatibilityLevel.NET_4_6);

            // Add Packages
            Client.AddAndRemove(new string[]
            {
                "com.unity.addressables",
                "com.unity.editorcoroutines",
                "com.unity.inputsystem"
            });
        }

        [MenuItem("Celeste/Bootstrap/2) Download Celeste", priority = 2)]
        public static void ExecuteDownloadCeleste()
        {
            string projectPath = Path.GetDirectoryName(Application.dataPath);

            // Add Celeste submodule
            ExecuteProcessTerminal("git", "init", projectPath);
            ExecuteProcessTerminal("git", "submodule add git@github.com:AlanWills/CelesteUnity.git Assets/Celeste", projectPath);
            ExecuteProcessTerminal("git", "submodule update --init --recursive", projectPath);

            AssetDatabase.Refresh();
        }

        #endregion

        #region Utility

        private static void ExecuteProcessTerminal(string executable, string argument, string workingDirectory)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = executable,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = false,
                    CreateNoWindow = false,
                    Arguments = argument,
                    WorkingDirectory = workingDirectory
                };

                Process myProcess = new Process()
                {
                    StartInfo = startInfo
                };

                myProcess.Start();
                myProcess.WaitForExit();
                myProcess.Close();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }

        #endregion
    }
}
