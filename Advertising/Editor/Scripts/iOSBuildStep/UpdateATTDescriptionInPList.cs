#if UNITY_IOS && UNITY_ATT
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

namespace CelesteEditor.Advertising
{
    public class UpdateATTDescriptionInPList
    {
        // Set the IDFA request description:
        const string k_TrackingDescription = "Your data will be used to provide you a better and personalized ad experience.";

        [PostProcessBuild(0)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToXcode)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                AddPListValues(pathToXcode);
            }
        }

        // Implement a function to read and write values to the plist file:
        private static void AddPListValues(string pathToXcode)
        {
            // Retrieve the plist file from the Xcode project directory:
            string plistPath = pathToXcode + "/Info.plist";
            PlistDocument plistObj = new PlistDocument();

            // Read the values from the plist file:
            plistObj.ReadFromString(File.ReadAllText(plistPath));

            // Set values from the root object:
            PlistElementDict plistRoot = plistObj.root;

            // Set the description key-value in the plist:
            plistRoot.SetString("NSUserTrackingUsageDescription", k_TrackingDescription);

            // Save changes to the plist:
            File.WriteAllText(plistPath, plistObj.WriteToString());
        }
    }
}
#endif