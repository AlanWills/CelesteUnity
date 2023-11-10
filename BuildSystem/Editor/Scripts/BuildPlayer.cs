using UnityEditor;

namespace CelesteEditor.BuildSystem
{
    public class BuildPlayer
    {
        #region Android

        [MenuItem("Celeste/Builds/Debug/Android", validate = true)]
        public static bool ValidateBuildDebugAndroidPlayer()
        {
            return AllPlatformSettings.GetOrCreateSettings().AndroidDebug != null;
        }

        [MenuItem("Celeste/Builds/Debug/Android", validate = false)]
        public static void BuildDebugAndroidPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().AndroidDebug.BuildPlayer();
            AllPlatformSettings.GetOrCreateSettings().AndroidDebug.IncrementBuild();
        }

        [MenuItem("Celeste/Builds/Release/Android Apk", validate = true)]
        public static bool ValidateBuildReleaseApkAndroidPlayer()
        {
            return AllPlatformSettings.GetOrCreateSettings().AndroidReleaseApk != null;
        }

        [MenuItem("Celeste/Builds/Release/Android Apk", validate = false)]
        public static void BuildReleaseApkAndroidPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().AndroidReleaseApk.BuildPlayer();
            AllPlatformSettings.GetOrCreateSettings().AndroidReleaseApk.IncrementBuild();
        }

        [MenuItem("Celeste/Builds/Release/Android Bundle", validate = true)]
        public static bool ValidateBuildReleaseBundleAndroidPlayer()
        {
            return AllPlatformSettings.GetOrCreateSettings().AndroidReleaseBundle != null;
        }

        [MenuItem("Celeste/Builds/Release/Android Bundle", validate = false)]
        public static void BuildReleaseBundleAndroidPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().AndroidReleaseBundle.BuildPlayer();
            AllPlatformSettings.GetOrCreateSettings().AndroidReleaseBundle.IncrementBuild();
        }

        #endregion

        #region Windows

        [MenuItem("Celeste/Builds/Debug/Windows", validate = true)]
        public static bool ValidateBuildDebugWindowsPlayer()
        {
            return AllPlatformSettings.GetOrCreateSettings().WindowsDebug != null;
        }

        [MenuItem("Celeste/Builds/Debug/Windows", validate = false)]
        public static void BuildDebugWindowsPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().WindowsDebug.BuildPlayer();
            AllPlatformSettings.GetOrCreateSettings().WindowsDebug.IncrementBuild();
        }

        [MenuItem("Celeste/Builds/Release/Windows", validate = true)]
        public static bool ValidateBuildReleaseWindowsPlayer()
        {
            return AllPlatformSettings.GetOrCreateSettings().WindowsRelease != null;
        }

        [MenuItem("Celeste/Builds/Release/Windows", validate = false)]
        public static void BuildReleaseWindowsPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().WindowsRelease.BuildPlayer();
            AllPlatformSettings.GetOrCreateSettings().WindowsRelease.IncrementBuild();
        }

        #endregion

        #region iOS

        [MenuItem("Celeste/Builds/Debug/iOS", validate = true)]
        public static bool ValidateBuildDebugiOSPlayer()
        {
            return AllPlatformSettings.GetOrCreateSettings().iOSDebug != null;
        }

        [MenuItem("Celeste/Builds/Debug/iOS", validate = false)]
        public static void BuildDebugiOSPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().iOSDebug.BuildPlayer();
            AllPlatformSettings.GetOrCreateSettings().iOSDebug.IncrementBuild();
        }

        [MenuItem("Celeste/Builds/Release/iOS", validate = true)]
        public static bool ValidateBuildReleaseiOSPlayer()
        {
            return AllPlatformSettings.GetOrCreateSettings().iOSRelease != null;
        }

        [MenuItem("Celeste/Builds/Release/iOS", validate = false)]
        public static void BuildReleaseiOSPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().iOSRelease.BuildPlayer();
            AllPlatformSettings.GetOrCreateSettings().iOSRelease.IncrementBuild();
        }

        #endregion

        #region WebGL

        [MenuItem("Celeste/Builds/Debug/WebGL", validate = true)]
        public static bool ValidateBuildDebugWebGLPlayer()
        {
            return AllPlatformSettings.GetOrCreateSettings().WebGLDebug != null;
        }

        [MenuItem("Celeste/Builds/Debug/WebGL", validate = false)]
        public static void BuildDebugWebGLPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().WebGLDebug.BuildPlayer();
            AllPlatformSettings.GetOrCreateSettings().WebGLDebug.IncrementBuild();
        }

        [MenuItem("Celeste/Builds/Release/WebGL", validate = true)]
        public static bool ValidateBuildReleaseWebGLPlayer()
        {
            return AllPlatformSettings.GetOrCreateSettings().WebGLRelease != null;
        }

        [MenuItem("Celeste/Builds/Release/WebGL", validate = false)]
        public static void BuildReleaseWebGLPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().WebGLRelease.BuildPlayer();
            AllPlatformSettings.GetOrCreateSettings().WebGLRelease.IncrementBuild();
        }

        #endregion
    }
}
