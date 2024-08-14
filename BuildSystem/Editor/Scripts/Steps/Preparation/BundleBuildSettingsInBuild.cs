#if USE_ADDRESSABLES
using Celeste;
using Celeste.BuildSystem;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    [CreateAssetMenu(
        fileName = nameof(BundleBuildSettingsInBuild), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Build Preparation/Bundle Build Settings In Build",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
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
#endif