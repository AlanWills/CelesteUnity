﻿using Celeste.Tools.Attributes.GUI;
using System.IO;
using System.Text;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    [CreateAssetMenu(fileName = nameof(WriteAssetEnvironmentVariablesToFile), menuName = "Celeste/Build System/Asset Post Process/Write Asset Environment Variables To File")]
    public class WriteAssetEnvironmentVariablesToFile : AssetPostProcessStep
    {
        #region Properties and Fields

        [SerializeField] private string assetEnvironmentVariablesFileName = "ASSETS_ENV_VARS.txt";

        [SerializeField] private bool writeAddressablesBuildDirectoryVariable = true;
        [SerializeField, ShowIf(nameof(writeAddressablesBuildDirectoryVariable))] private string addressablesBuildDirectoryVariableName = "ASSETS_SOURCE";

        [SerializeField] private bool writeAddressablesUploadURLVariable = true;
        [SerializeField, ShowIf(nameof(writeAddressablesUploadURLVariable))] private string addressablesUploadURLVariableName = "ASSETS_DESTINATION";

        #endregion

        public override void Execute(AddressablesPlayerBuildResult result, PlatformSettings platformSettings)
        {
            StringBuilder locationInfo = new StringBuilder();
            locationInfo.Append($"{addressablesBuildDirectoryVariableName}={platformSettings.AddressablesBuildDirectory}");
            locationInfo.AppendLine();
            locationInfo.Append($"{addressablesUploadURLVariableName}={platformSettings.AddressablesUploadURL}");

            if (!Directory.Exists(platformSettings.AddressablesBuildDirectory))
            {
                Directory.CreateDirectory(platformSettings.AddressablesBuildDirectory);
            }

            File.WriteAllText(Path.Combine(new DirectoryInfo(platformSettings.AddressablesBuildDirectory).Parent.FullName, assetEnvironmentVariablesFileName), locationInfo.ToString());
        }
    }
}