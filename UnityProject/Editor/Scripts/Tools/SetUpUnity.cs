using System;
using System.Diagnostics;
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
            // Clone Repo and Update Submodules
            ExecuteProcessTerminal("git", "clone --recurse-submodules --remote-submodules git@github.com:AlanWills/CelesteUnity.git Celeste");
        }

        #endregion

        #region Utility

        private static Tuple<string, string, int> ExecuteProcessTerminalReturnAll(string executable, string argument)
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
                    CreateNoWindow = true,
                    Arguments = argument,
                    WorkingDirectory = Application.dataPath
                };

                Process myProcess = new Process()
                {
                    StartInfo = startInfo
                };

                try
                {
                    myProcess.Start();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError("Git is not set-up correctly, required to be on PATH, and to be a git project.");
                    throw e;
                }

                myProcess.WaitForExit();
                myProcess.Close();

                return new Tuple<string, string, int>(string.Empty, string.Empty, myProcess.ExitCode);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
                return new Tuple<string, string, int>(null, e.ToString(), -1);
            }
        }

        private static string ExecuteProcessTerminal(string executable, string argument)
        {
            var data = ExecuteProcessTerminalReturnAll(executable, argument);
            var output = data.Item1;
            var error = data.Item2;
            var returnCode = data.Item3;

            if (returnCode == -1)
            {
                return null;
            }
            else
            {
                return output;
            }
        }

        #endregion
    }
}
