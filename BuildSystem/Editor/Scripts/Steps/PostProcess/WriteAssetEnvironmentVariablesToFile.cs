using Celeste;
using Celeste.Tools.Attributes.GUI;
using System.IO;
using System.Text;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    [CreateAssetMenu(
        fileName = nameof(WriteAssetEnvironmentVariablesToFile), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Asset Post Process/Write Asset Environment Variables To File",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class WriteAssetEnvironmentVariablesToFile : AssetPostProcessStep
    {
        #region Properties and Fields

        [SerializeField] private string assetEnvironmentVariablesFileName = "ASSETS_ENV_VARS.txt";

        [SerializeField] private bool writeAddressablesBuildDirectoryVariable = true;
        [SerializeField, ShowIf(nameof(writeAddressablesBuildDirectoryVariable))] private string addressablesBuildDirectoryVariableName = "ASSETS_SOURCE";

        [SerializeField] private bool writeAddressablesUploadURLVariable = true;
        [SerializeField, ShowIf(nameof(writeAddressablesUploadURLVariable))] private string addressablesUploadURLVariableName = "ASSETS_DESTINATION";

        [SerializeField, ShowIf(nameof(writeAddressablesUploadURLVariable))] private bool writeAddressablesUploadCredentialsVariable = true;
        [SerializeField, ShowIfAll(nameof(writeAddressablesUploadURLVariable), nameof(writeAddressablesUploadCredentialsVariable))] private string addressablesUploadCredentialsVariableName = "ASSETS_UPLOAD_CREDENTIALS";

        #endregion

        public override void Execute(AddressablesPlayerBuildResult result, PlatformSettings platformSettings)
        {
            StringBuilder locationInfo = new StringBuilder();

            if (writeAddressablesBuildDirectoryVariable)
            {
                locationInfo.AppendLine($"{addressablesBuildDirectoryVariableName}={platformSettings.AddressablesBuildDirectory}");
            }

            if (writeAddressablesUploadURLVariable)
            {
                locationInfo.AppendLine($"{addressablesUploadURLVariableName}={platformSettings.AddressablesUploadURL}");

                if (writeAddressablesUploadCredentialsVariable)
                {
                    locationInfo.AppendLine($"{addressablesUploadURLVariableName}={platformSettings.AddressablesUploadURL}");
                }
            }

            if (!Directory.Exists(platformSettings.AddressablesBuildDirectory))
            {
                Directory.CreateDirectory(platformSettings.AddressablesBuildDirectory);
            }

            string parentDirectory = new DirectoryInfo(platformSettings.AddressablesBuildDirectory).Parent.FullName;
            Directory.CreateDirectory(parentDirectory);
            File.WriteAllText(Path.Combine(parentDirectory, assetEnvironmentVariablesFileName), locationInfo.ToString());
        }
    }
}
