using System.IO;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    [CreateAssetMenu(fileName = nameof(DeleteAddressablesRemoteDirectory), menuName = "Celeste/Build System/Asset Preparation/Delete Addressables Remote Directory")]
    public class DeleteAddressablesRemoteDirectory : AssetPreparationStep
    {
        public override void Execute()
        {
            string buildDir = GetAddressablesRemoteBuildDir();

            if (Directory.Exists(buildDir))
            {
                Directory.Delete(buildDir, true);
            }
        }

        private static string GetAddressablesRemoteBuildDir()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var profileSettings = settings.profileSettings;
            var propName = profileSettings.GetValueByName(settings.activeProfileId, "RemoteBuildPath");
            return propName;
        }
    }
}
