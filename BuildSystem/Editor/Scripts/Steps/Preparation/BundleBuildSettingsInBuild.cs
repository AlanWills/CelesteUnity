using Celeste.BuildSystem;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    [CreateAssetMenu(fileName = nameof(BundleBuildSettingsInBuild), menuName = "Celeste/Build System/Build Preparation/Bundle Build Settings In Build")]
    public class BundleBuildSettingsInBuild : BuildPreparationStep
    {
        public override void Execute(BuildPlayerOptions buildPlayerOptions, PlatformSettings platformSettings)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "BuildSettings.json");
            string baseUrl = $"{platformSettings.AddressablesLoadDirectory}/catalog_{platformSettings.PlayerOverrideVersion}";

            RuntimeBuildSettings runtimeBuildSettings = new RuntimeBuildSettings()
            {
                RemoteContentCatalogueHashURL = $"{baseUrl}.hash",
                RemoteContentCatalogueJsonURL = $"{baseUrl}.json",
            };

            File.WriteAllText(filePath, JsonUtility.ToJson(runtimeBuildSettings));
        }
    }
}
