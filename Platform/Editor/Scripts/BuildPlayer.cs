using UnityEditor;

namespace CelesteEditor.BuildSystem
{
    public static class BuildPlayer
    {
        [MenuItem("Celeste/Builds/Debug/Android")]
        public static void BuildDebugAndroidPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().AndroidDebug.BuildPlayer();
        }

        [MenuItem("Celeste/Builds/Release/Android Apk")]
        public static void BuildReleaseAndroidApkPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().AndroidReleaseApk.BuildPlayer();
        }

        [MenuItem("Celeste/Builds/Release/Android Bundle")]
        public static void BuildReleaseAndroidPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().AndroidReleaseBundle.BuildPlayer();
        }

        [MenuItem("Celeste/Builds/Debug/Windows")]
        public static void BuildDebugWindowsPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().WindowsDebug.BuildPlayer();
        }

        [MenuItem("Celeste/Builds/Release/Windows")]
        public static void BuildReleaseWindowsPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().WindowsRelease.BuildPlayer();
        }

        [MenuItem("Celeste/Builds/Debug/iOS")]
        public static void BuildDebugiOSPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().iOSDebug.BuildPlayer();
        }

        [MenuItem("Celeste/Builds/Release/iOS")]
        public static void BuildReleaseiOSPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().iOSRelease.BuildPlayer();
        }

        [MenuItem("Celeste/Builds/Debug/WebGL")]
        public static void BuildDebugWebGLPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().WebGLDebug.BuildPlayer();
        }

        [MenuItem("Celeste/Builds/Release/WebGL")]
        public static void BuildReleaseWebGLPlayer()
        {
            AllPlatformSettings.GetOrCreateSettings().WebGLRelease.BuildPlayer();
        }
    }
}
