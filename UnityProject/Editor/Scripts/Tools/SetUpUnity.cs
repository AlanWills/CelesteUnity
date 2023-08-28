using UnityEditor;
using UnityEditor.PackageManager;

namespace CelesteEditor.UnityProject
{
    public static class SetUpUnity
    {
        [MenuItem("Celeste/Bootstrap/1) Set Up Unity", priority = 1)]
        public static void ExecuteSetUpUnity()
        {
            // Change .Net Framework
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_4_6);
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone, ApiCompatibilityLevel.NET_4_6);
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WebGL, ApiCompatibilityLevel.NET_4_6);

            // Add and Remove relevant packages
            Client.AddAndRemove(
                new string[]
                {
                    // To add
                    "git@github.com:AlanWills/UnityRuntimeInspector.git",
                    "git@github.com:AlanWills/UnityNativeShare.git",
                    "git@github.com:AlanWills/UnityNativeFilePicker.git"
                },
                new string[]
                {
                    // To remove
                    "com.unity.collab-proxy"
                });
        }
    }
}
