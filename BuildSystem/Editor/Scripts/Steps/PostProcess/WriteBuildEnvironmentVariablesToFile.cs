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

        [SerializeField] private bool writeVersionVariable = true;
        [SerializeField, ShowIf(nameof(writeVersionVariable))] private string versionVariableName = "BUILD_VERSION";

        [SerializeField] private bool writeUploadDirectoryVariable = true;
        [SerializeField, ShowIf(nameof(writeUploadDirectoryVariable))] private string uploadDirectoryVariableName = "BUILD_UPLOAD_DIRECTORY";

        #endregion

        public override void Execute(BuildPlayerOptions buildPlayerOptions, BuildReport result, PlatformSettings platformSettings)
        {
            StringBuilder buildInfo = new StringBuilder();
            if (writeBuildDirectoryVariable)
            {
                buildInfo.Append($"{buildDirectoryVariableName}={platformSettings.BuildDirectory}");
                buildInfo.AppendLine();
            }

            if (writeBuildLocationVariable)
            {
                buildInfo.Append($"{buildLocationVariableName}={buildPlayerOptions.locationPathName}");
                buildInfo.AppendLine();
            }

            if (writeVersionVariable)
            {
                buildInfo.Append($"{versionVariableName}={platformSettings.Version}");
                buildInfo.AppendLine();
            }

            if (writeUploadDirectoryVariable)
            {
                buildInfo.Append($"{uploadDirectoryVariableName}={platformSettings.BuildUploadURL}");
            }

            platformSettings.InjectBuildEnvVars(buildInfo);

            Directory.CreateDirectory(platformSettings.BuildDirectory);
            File.WriteAllText(Path.Combine(platformSettings.BuildDirectory, $"{buildEnvironmentVariablesFileName}"), buildInfo.ToString());
        }
    }
}
