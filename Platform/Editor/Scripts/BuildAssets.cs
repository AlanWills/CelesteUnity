using UnityEditor;
using UnityEngine;
using static UnityEngine.Application;

namespace CelesteEditor.BuildSystem
{
    public static class BuildAssets
    {
        [MenuItem("Celeste/Assets/Debug/Build Android Assets")]
        public static void BuildDebugAndroidAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().AndroidDebug.BuildAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Build Android Assets")]
        public static void BuildReleaseAndroidAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().AndroidReleaseBundle.BuildAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Debug/Build Windows Assets")]
        public static void BuildDebugWindowsAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WindowsDebug.BuildAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Build Windows Assets")]
        public static void BuildReleaseWindowsAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WindowsRelease.BuildAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Debug/Build iOS Assets")]
        public static void BuildDebugiOSAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().iOSDebug.BuildAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Build iOS Assets")]
        public static void BuildReleaseiOSAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().iOSRelease.BuildAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Debug/Build WebGL Assets")]
        public static void BuildDebugWebGLAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WebGLDebug.BuildAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Build WebGL Assets")]
        public static void BuildReleaseWebGLAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WebGLRelease.BuildAssetsAndExit();
        }

        private static void BuildAssetsAndExit(this PlatformSettings platformSettings)
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
            platformSettings.BuildAssets();
            Application.logMessageReceived -= logDelegate;

            if (hasThrownAnException)
            {
                Debug.LogErrorFormat("Exception thrown during asset building: {0}", exceptionMessage);
            }

            if (Application.isBatchMode)
            {
                // 0 = everything OK
                // 1 = everything NOT OK
                EditorApplication.Exit(hasThrownAnException ? 1 : 0);
            }
            else
            {
                EditorUtility.DisplayDialog("Asset Build result", string.Format("Assets Built {0}", hasThrownAnException ? "Unsuccessfully" : "Successfully"), "OK");
            }
        }
    }
}
