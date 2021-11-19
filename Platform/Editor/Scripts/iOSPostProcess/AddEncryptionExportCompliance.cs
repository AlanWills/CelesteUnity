using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace CelesteEditor.Platform.iOSPostProcess
{
    public static class AddEncryptionExportCompliance
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
                var buildKeyExportCompliance = "ITSAppUsesNonExemptEncryption";
                rootDict.SetString(buildKeyExportCompliance, "false");

                // Write to file
                File.WriteAllText(plistPath, plist.WriteToString());
            }
        }
    }
}