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

        [SerializeField] private bool writeAddressablesUploadVariables = true;
        [SerializeField, ShowIf(nameof(writeAddressablesUploadVariables))] private string addressablesUploadBucketVariableName = "ASSETS_UPLOAD_BUCKET";
        [SerializeField, ShowIf(nameof(writeAddressablesUploadVariables))] private string addressablesUploadPathVariableName = "ASSETS_UPLOAD_PATH";

        [SerializeField, ShowIf(nameof(writeAddressablesUploadVariables))] private bool writeAddressablesUploadCredentialsVariable = true;
        [SerializeField, ShowIfAll(nameof(writeAddressablesUploadVariables), nameof(writeAddressablesUploadCredentialsVariable))] private string addressablesUploadCredentialsVariableName = "ASSETS_UPLOAD_CREDENTIALS";

        #endregion

        public override void Execute(AddressablesPlayerBuildResult result, PlatformSettings platformSettings)
        {
            StringBuilder locationInfo = new StringBuilder();

            if (writeAddressablesBuildDirectoryVariable)
            {
                locationInfo.AppendLine($"{addressablesBuildDirectoryVariableName}={platformSettings.AddressablesBuildDirectory}");
            }

            if (writeAddressablesUploadVariables)
            {
                locationInfo.AppendLine($"{addressablesUploadBucketVariableName}={platformSettings.AddressablesUploadBucket}");
                locationInfo.AppendLine($"{addressablesUploadPathVariableName}={platformSettings.AddressablesUploadPath}");

                if (writeAddressablesUploadCredentialsVariable)
                {
                    locationInfo.AppendLine($"{addressablesUploadCredentialsVariableName}={platformSettings.AddressablesUploadCredentials}");
                }
            }

            if (!Directory.Exists(platformSettings.AddressablesBuildDirectory))
            {
                Directory.CreateDirectory(platformSettings.AddressablesBuildDirectory);
            }

            string libraryDirectory = Path.Combine(new DirectoryInfo(Application.dataPath).Parent.FullName, "Library");
            Directory.CreateDirectory(libraryDirectory);
            File.WriteAllText(Path.Combine(libraryDirectory, assetEnvironmentVariablesFileName), locationInfo.ToString());
        }
    }
}
