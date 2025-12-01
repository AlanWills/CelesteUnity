using Celeste.Tools.Attributes.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace CelesteEditor.UnityProject
{
    [Serializable]
    public struct BootstrapCelesteParameters
    {
        [LabelWidth(300)] public bool useAddressables;
        [LabelWidth(300)] public bool useNewInputSystem;
        [LabelWidth(300)] public bool useTextMeshPro;
        [LabelWidth(300)] public bool useUnityAndroidLogcatPackage;
        [LabelWidth(300)] public bool removeUnityCollabPackage;
        [LabelWidth(300)] public bool useNetCodeForGameObjects;

        public void UseDefaults()
        {
            useAddressables = true;
            useNewInputSystem = true;
            useTextMeshPro = true;
            useUnityAndroidLogcatPackage = true;
            removeUnityCollabPackage = true;
            useNetCodeForGameObjects = false;
        }
    }

    public static class BootstrapCeleste
    {
        #region Properties and Fields

        public static bool IsStarted => EditorPrefs.GetBool(BootstrapCelesteConstants.BOOTSTRAP_COMPLETED_EDITOR_PREFS_KEY, false);

        private static AddAndRemoveRequest s_addAndRemoveRequest;
        private static BootstrapCelesteParameters s_parameters;

        #endregion

        public static void Execute(BootstrapCelesteParameters parameters)
        {
            s_parameters = parameters;
            
            if (parameters.useTextMeshPro)
            {
                string packageFullPath = TMP_EditorUtility.packageFullPath;
                AssetDatabase.ImportPackage($"{packageFullPath}/Package Resources/TMP Essential Resources.unitypackage", false);
            }

            List<string> dependenciesToAdd = new List<string>
            {
                BootstrapCelesteConstants.EDITOR_COROUTINES_PACKAGE
            };
            
            List<string> dependenciesToRemove = new List<string>();

            if (parameters.useAddressables)
            {
                dependenciesToAdd.Add(BootstrapCelesteConstants.ADDRESSABLES_PACKAGE);
            }

            if (parameters.useNewInputSystem)
            {
                dependenciesToAdd.Add(BootstrapCelesteConstants.NEW_INPUT_SYSTEM_PACKAGE);
            }

            if (parameters.useNetCodeForGameObjects)
            {
                dependenciesToAdd.Add(BootstrapCelesteConstants.NET_CODE_FOR_GAMEOBJECTS_PACKAGE);
                dependenciesToAdd.Add(BootstrapCelesteConstants.MULTIPLAYER_SERVICES_PACKAGE);
            }
            
            if (parameters.useUnityAndroidLogcatPackage)
            {
                dependenciesToAdd.Add(BootstrapCelesteConstants.ANDROID_LOGCAT_PACKAGE);
            }

            if (parameters.removeUnityCollabPackage)
            {
                dependenciesToRemove.Add(BootstrapCelesteConstants.UNITY_COLLAB_PACKAGE);
            }

            if (dependenciesToAdd.Count > 0)
            {
                s_addAndRemoveRequest = Client.AddAndRemove(packagesToAdd: dependenciesToAdd.ToArray(), packagesToRemove: dependenciesToRemove.ToArray());
                UnityEngine.Debug.Log("Beginning bootstrap of Celeste dependencies...");

                if (!s_addAndRemoveRequest.IsCompleted)
                {
                    EditorApplication.update += UpdateProgressBar;
                    EditorUtility.DisplayProgressBar("Bootstrapping", "Please wait, downloading necessary packages...", 0f);
                }
                else
                {
                    OnBootstrapComplete();
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
                    UnityEngine.Debug.LogError($"There was an issue bootstrapping dependencies ({s_addAndRemoveRequest.Error.message}).  You may have to do this yourself...");
                }

                EditorUtility.ClearProgressBar();
                EditorApplication.update -= UpdateProgressBar;

                OnBootstrapComplete();
            }
            else
            {
                EditorUtility.DisplayProgressBar("Bootstrapping", "Please wait, downloading necessary packages...", 0f);
            }
        }

        private static void OnBootstrapComplete()
        {
            s_addAndRemoveRequest = null;
            EditorPrefs.SetBool(BootstrapCelesteConstants.BOOTSTRAP_COMPLETED_EDITOR_PREFS_KEY, true);

            // Restart the editor to allow everything to have a nice clean start (some packages also require a restart e.g. new input system)
            // Currently doesn't seem to play nice with the new input system, so only do this if we're not using the new input system
            if (!s_parameters.useNewInputSystem)
            {
                EditorApplication.OpenProject(Directory.GetCurrentDirectory());
            }

            s_parameters = default;
        }
    }
}