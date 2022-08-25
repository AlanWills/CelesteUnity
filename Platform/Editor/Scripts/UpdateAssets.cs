using UnityEditor;
using UnityEngine;
using static UnityEngine.Application;

namespace CelesteEditor.BuildSystem
{
    public static class UpdateAssets
    {
        #region Android

        [MenuItem("Celeste/Assets/Debug/Update Android Assets", validate = true)]
        public static bool ValidateUpdateDebugAndroidAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().AndroidDebug != null;
        }

        [MenuItem("Celeste/Assets/Debug/Update Android Assets", validate = false)]
        public static void UpdateDebugAndroidAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().AndroidDebug.UpdateAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Update Android Assets", validate = true)]
        public static bool ValidateUpdateReleaseAndroidAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().AndroidReleaseBundle != null;
        }

        [MenuItem("Celeste/Assets/Release/Update Android Assets", validate = false)]
        public static void UpdateReleaseAndroidAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().AndroidReleaseBundle.UpdateAssetsAndExit();
        }

        #endregion

        #region Windows

        [MenuItem("Celeste/Assets/Debug/Update Windows Assets", validate = true)]
        public static bool ValidateUpdateDebugWindowsAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().WindowsDebug != null;
        }

        [MenuItem("Celeste/Assets/Debug/Update Windows Assets", validate = false)]
        public static void UpdateDebugWindowsAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WindowsDebug.UpdateAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Update Windows Assets", validate = true)]
        public static bool ValidateUpdateReleaseWindowsAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().WindowsRelease != null;
        }

        [MenuItem("Celeste/Assets/Release/Update Windows Assets", validate = false)]
        public static void UpdateReleaseWindowsAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WindowsRelease.UpdateAssetsAndExit();
        }

        #endregion

        #region iOS

        [MenuItem("Celeste/Assets/Debug/Update iOS Assets", validate = true)]
        public static bool ValidateUpdateDebugiOSAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().iOSDebug != null;
        }

        [MenuItem("Celeste/Assets/Debug/Update iOS Assets", validate = false)]
        public static void UpdateDebugiOSAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().iOSDebug.UpdateAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Update iOS Assets", validate = true)]
        public static bool ValidateUpdateReleaseiOSAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().iOSRelease != null;
        }

        [MenuItem("Celeste/Assets/Release/Update iOS Assets", validate = false)]
        public static void UpdateReleaseiOSAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().iOSRelease.UpdateAssetsAndExit();
        }

        #endregion

        #region WebGL

        [MenuItem("Celeste/Assets/Debug/Update WebGL Assets", validate = true)]
        public static bool ValidateUpdateDebugWebGLAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().WebGLDebug != null;
        }

        [MenuItem("Celeste/Assets/Debug/Update WebGL Assets", validate = false)]
        public static void UpdateDebugWebGLAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WebGLDebug.UpdateAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Update WebGL Assets", validate = true)]
        public static bool ValidateUpdateReleaseWebGLAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().WebGLRelease != null;
        }

        [MenuItem("Celeste/Assets/Release/Update WebGL Assets", validate = false)]
        public static void UpdateReleaseWebGLAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WebGLRelease.UpdateAssetsAndExit();
        }

        #endregion

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
