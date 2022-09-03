using Celeste.Tools.Attributes.GUI;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    [CreateAssetMenu(fileName = nameof(WriteBuildEnvironmentVariablesToFile), menuName = "Celeste/Build System/Build Post Process/Write Build Environment Variables To File")]
    public class WriteBuildEnvironmentVariablesToFile : BuildPostProcessStep
    {
        #region Properties and Fields

        [SerializeField] private string buildEnvironmentVariablesFileName = "BUILD_ENV_VARS.txt";

        [SerializeField] private bool writeBuildLocationVariable = true;
        [SerializeField, ShowIf(nameof(writeBuildLocationVariable))] private string buildLocationVariableName = "BUILD_LOCATION";

        [SerializeField] private bool writeVersionVariable = true;
        [SerializeField, ShowIf(nameof(writeVersionVariable))] private string versionVariableName = "BUILD_VERSION";

        [SerializeField] private bool writeUploadDirectoryVariable = true;
        [SerializeField, ShowIf(nameof(writeUploadDirectoryVariable))] private string uploadDirectoryVariableName = "BUILD_GDRIVE_UPLOAD_DIRECTORY";

        #endregion

        public override void Execute(BuildPlayerOptions buildPlayerOptions, BuildReport result, PlatformSettings platformSettings)
        {
            StringBuilder buildInfo = new StringBuilder();
            buildInfo.Append($"{buildLocationVariableName}={buildPlayerOptions.locationPathName}");
            buildInfo.AppendLine();
            buildInfo.Append($"{versionVariableName}={platformSettings.Version}");
            buildInfo.AppendLine();
            buildInfo.Append($"{uploadDirectoryVariableName}={platformSettings.BuildUploadURL}");

            platformSettings.InjectBuildEnvVars(buildInfo);

            File.WriteAllText(Path.Combine(platformSettings.BuildDirectory, "buildEnvironmentVariablesFileName.txt"), buildInfo.ToString());
        }
    }
}
