using UnityEditor;
using UnityEngine;
using static UnityEngine.Application;

namespace CelesteEditor.BuildSystem
{
    public static class UpdateAssets
    {
        [MenuItem("Celeste/Assets/Debug/Update Android Assets")]
        public static void UpdateDebugAndroidAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().AndroidDebug.UpdateAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Update Android Assets")]
        public static void UpdateReleaseAndroidAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().AndroidReleaseBundle.UpdateAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Debug/Update Windows Assets")]
        public static void UpdateDebugWindowsAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WindowsDebug.UpdateAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Update Windows Assets")]
        public static void UpdateReleaseWindowsAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WindowsRelease.UpdateAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Debug/Update iOS Assets")]
        public static void UpdateDebugiOSAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().iOSDebug.UpdateAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Update iOS Assets")]
        public static void UpdateReleaseiOSAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().iOSRelease.UpdateAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Debug/Update WebGL Assets")]
        public static void UpdateDebugWebGLAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WebGLDebug.UpdateAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Update HTML5 Assets")]
        public static void UpdateReleaseWebGLAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WebGLRelease.UpdateAssetsAndExit();
        }

        private static void UpdateAssetsAndExit(this PlatformSettings platformSettings)
        {
            string exceptionMessage = "";
            bool hasThrownAnException = false;
            LogCallback logDelegate = (string logString, string stackTrace, LogType type) =>
            {
                if (type == LogType.Exception)
                {
                    hasThrownAnException = true;
                    exceptionMessage = logString;
                }
            };
            Application.logMessageReceived += logDelegate;
            platformSettings.UpdateAssets();
            Application.logMessageReceived -= logDelegate;

            if (hasThrownAnException)
            {
                Debug.LogErrorFormat("Exception thrown during asset updating: {0}", exceptionMessage);
            }

            if (Application.isBatchMode)
            {
                // 0 = everything OK
                // 1 = everything NOT OK
                EditorApplication.Exit(hasThrownAnException ? 1 : 0);
            }
            else
            {
                EditorUtility.DisplayDialog("Asset Update result", string.Format("Assets Update {0}", hasThrownAnException ? "Unsuccessfully" : "Successfully"), "OK");
            }
        }
    }
}
