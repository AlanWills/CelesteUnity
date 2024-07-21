using Celeste;
using Celeste.Tools.Attributes.GUI;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    [CreateAssetMenu(
        fileName = nameof(WriteBuildEnvironmentVariablesToFile), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Build Post Process/Write Build Environment Variables To File",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class WriteBuildEnvironmentVariablesToFile : BuildPostProcessStep
    {
        #region Properties and Fields

        [SerializeField] private string buildEnvironmentVariablesFileName = "BUILD_ENV_VARS.txt";

        [SerializeField] private bool writeBuildDirectoryVariable = true;
        [SerializeField, ShowIf(nameof(writeBuildDirectoryVariable))] private string buildDirectoryVariableName = "BUILD_DIRECTORY";

        [SerializeField] private bool writeBuildLocationVariable = true;
        [SerializeField, ShowIf(nameof(writeBuildLocationVariable))] private string buildLocationVariableName = "BUILD_LOCATION";

        [SerializeField] private bool writeBuildOutputNameVariable = true;
        [SerializeField, ShowIf(nameof(writeBuildOutputNameVariable))] private string buildOutputNameVariableName = "BUILD_OUTPUT_NAME";

        [SerializeField] private bool writeVersionVariable = true;
        [SerializeField, ShowIf(nameof(writeVersionVariable))] private string versionVariableName = "BUILD_VERSION";

        [SerializeField] private bool writeUploadVariables = true;
        [SerializeField, ShowIf(nameof(writeUploadVariables))] private string buildUploadURLVariableName = "BUILD_UPLOAD_URL";

        [SerializeField, ShowIf(nameof(writeUploadVariables))] private bool writeBuildUploadCredentialsVariable = true;
        [SerializeField, ShowIfAll(nameof(writeUploadVariables), nameof(writeBuildUploadCredentialsVariable))] private string buildUploadCredentialsVariableName = "BUILD_UPLOAD_CREDENTIALS";

        #endregion

        public override void Execute(BuildPlayerOptions buildPlayerOptions, BuildReport result, PlatformSettings platformSettings)
        {
            StringBuilder buildInfo = new StringBuilder();
            if (writeBuildDirectoryVariable)
            {
                buildInfo.AppendLine($"{buildDirectoryVariableName}={platformSettings.BuildDirectory}");
            }

            if (writeBuildLocationVariable)
            {
                buildInfo.AppendLine($"{buildLocationVariableName}={buildPlayerOptions.locationPathName}");
            }

            if (writeBuildOutputNameVariable)
            {
                buildInfo.AppendLine($"{buildOutputNameVariableName}={Path.GetFileName(buildPlayerOptions.locationPathName)}");
            }

            if (writeVersionVariable)
            {
                buildInfo.AppendLine($"{versionVariableName}={platformSettings.Version}");
            }

            if (writeUploadVariables)
            {
                buildInfo.AppendLine($"{buildUploadURLVariableName}={platformSettings.BuildUploadURL}");

                if (writeBuildUploadCredentialsVariable)
                {
                    buildInfo.AppendLine($"{buildUploadCredentialsVariableName}={platformSettings.BuildUploadCredentials}");
                }
            }

            platformSettings.InjectBuildEnvVars(buildInfo);

            string libraryDirectory = Path.Combine(new DirectoryInfo(Application.dataPath).Parent.FullName, "Library");
            Directory.CreateDirectory(libraryDirectory);
            File.WriteAllText(Path.Combine(libraryDirectory, $"{buildEnvironmentVariablesFileName}"), buildInfo.ToString());
        }
    }
}
