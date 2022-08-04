using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace CelesteEditor.BuildSystem.iOSPostProcess
{
    public static class DisableAlwaysEmbedSwiftStandardLibraries
    {
        [PostProcessBuild(999)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                PBXProject project = new PBXProject();
                string projPath = PBXProject.GetPBXProjectPath(path);
                project.ReadFromFile(projPath);

                string mainTargetGuid = project.GetUnityMainTargetGuid();
                string unityFrameworkTargetGuid = project.GetUnityFrameworkTargetGuid();

                project.SetBuildProperty(mainTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
                project.SetBuildProperty(unityFrameworkTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");

                project.WriteToFile(projPath);
            }
        }
    }
}