#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace CelesteEditor.LocationServices.iOSPostProcess
{
    public static class AddMotionUsageDescription
    {
        [PostProcessBuild(999)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                // Get plist
                string plistPath = $"{path}/Info.plist";
                var plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(plistPath));

                // Get root
                var rootDict = plist.root;

                // Add export compliance for TestFlight builds
                rootDict.SetString("NSMotionUsageDescription", "Use location to determine step count.");

                // Write to file
                File.WriteAllText(plistPath, plist.WriteToString());
            }
        }
    }
}
#endif