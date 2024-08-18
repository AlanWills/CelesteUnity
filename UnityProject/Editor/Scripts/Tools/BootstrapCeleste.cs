using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace CelesteEditor.UnityProject
{
    [Serializable]
    public struct BootstrapCelesteParameters
    {
        public bool useAddressables;
        public bool useNewInputSystem;

        public void UseDefaults()
        {
            useAddressables = true;
            useNewInputSystem = true;
        }
    }

    public static class BootstrapCeleste
    {
        private static AddAndRemoveRequest s_addAndRemoveRequest;

        public static void Execute(BootstrapCelesteParameters parameters)
        {
            List<string> dependenciesToAdd = new List<string>
            {
                BootstrapCelesteConstants.EDITOR_COROUTINES_PACKAGE
            };

            if (parameters.useAddressables)
            {
                dependenciesToAdd.Add(BootstrapCelesteConstants.ADDRESSABLES_PACKAGE);
            }

            if (parameters.useNewInputSystem)
            {
                dependenciesToAdd.Add(BootstrapCelesteConstants.NEW_INPUT_SYSTEM_PACKAGE);
            }

            if (dependenciesToAdd.Count > 0)
            {
                s_addAndRemoveRequest = Client.AddAndRemove(packagesToAdd: dependenciesToAdd.ToArray());
                UnityEngine.Debug.Log("Beginning bootstrap of Celeste dependencies...");

                if (!s_addAndRemoveRequest.IsCompleted)
                {
                    EditorApplication.update += UpdateProgressBar;
                    EditorUtility.DisplayProgressBar("Bootstrapping", "Please wait, downloading necessary packages...", 0f);
                }
            }
        }

        private static void UpdateProgressBar()
        {
            if (s_addAndRemoveRequest.IsCompleted)
            {
                if (s_addAndRemoveRequest.Status == StatusCode.Success)
                {
                    UnityEngine.Debug.Log("Bootstrapping of dependencies completed successfully!");
                }
                else
                {
                    UnityEngine.Debug.LogError("There was an issue bootstrapping dependencies.  You may have to do this yourself...");
                }

                s_addAndRemoveRequest = null;
                EditorUtility.ClearProgressBar();
                EditorApplication.update -= UpdateProgressBar;
            }
            else
            {
                EditorUtility.DisplayProgressBar("Bootstrapping", "Please wait, downloading necessary packages...", 0f);
            }
        }
    }
}