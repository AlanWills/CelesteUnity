using Celeste.Constants;
using Celeste.Parameters;
using System.IO;
using UnityEngine;

namespace Celeste.Application
{
    public static class ApplicationUtils
    {
        public static void GetIsEditor(this BoolValue isEditor)
        {
            if (isEditor != null)
            {
                isEditor.Value = UnityEngine.Application.isEditor;
                UnityEngine.Debug.Log($"isEditor set to {isEditor.Value}");
            }
        }

        public static void GetIsMobile(this BoolValue isMobile)
        {
            if (isMobile != null)
            {
#if UNITY_EDITOR
                var buildTarget = UnityEditor.EditorUserBuildSettings.activeBuildTarget;
                isMobile.Value = buildTarget == UnityEditor.BuildTarget.Android ||
                                 buildTarget == UnityEditor.BuildTarget.iOS;
#else
                isMobile.Value = UnityEngine.Application.isMobilePlatform;
#endif
                UnityEngine.Debug.Log($"isMobile set to {isMobile.Value}");
            }
        }

        public static void GetIsDebugBuild(this BoolValue isDebugBuild)
        {
            if (isDebugBuild != null)
            {
                TextAsset textAsset = Resources.Load<TextAsset>(DebugConstants.IS_DEBUG_BUILD_FILE);
                isDebugBuild.Value = textAsset != null && textAsset.text == "1";
                UnityEngine.Debug.LogFormat("IS_DEBUG_BUILD_FILE file {0} ", textAsset != null ? "found with contents " + textAsset.text : "not found");
                UnityEngine.Debug.LogFormat("isDebugBuild set to {0}", isDebugBuild.Value);

                string settingsOverrideFile = Path.Combine(UnityEngine.Application.persistentDataPath, DebugConstants.IS_DEBUG_BUILD_FILE + ".txt");
                if (File.Exists(settingsOverrideFile))
                {
                    // The override file is present in persistent data - we want to change this build to debug after all
                    string fileContents = File.ReadAllText(settingsOverrideFile);
                    isDebugBuild.Value = fileContents == "1" ? true : isDebugBuild.Value;
                    UnityEngine.Debug.LogFormat("IS_DEBUG_BUILD_FILE in pdp found with contents {0}", fileContents);
                    UnityEngine.Debug.LogFormat("isDebugBuild set to {0}", isDebugBuild.Value);
                }
            }
        }
    }
}
