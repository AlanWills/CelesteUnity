using Celeste.BuildSystem;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    [CreateAssetMenu(fileName = nameof(BundleRemoteAddressablesInBuild), menuName = "Celeste/Build System/Asset Post Process/Bundle Remote Addressables In Build")]
    public class BundleRemoteAddressablesInBuild : AssetPostProcessStep
    {
        public override void Execute(AddressablesPlayerBuildResult result, PlatformSettings platformSettings)
        {
            CacheRemoteAddressablesBundleList(result);
            BundleRemoteAddressables();
        }

        #region Utility

        private static string GetAddressablesRemoteBuildDir()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var profileSettings = settings.profileSettings;
            var propName = profileSettings.GetValueByName(settings.activeProfileId, "RemoteBuildPath");
            return propName;
        }

        private static string GetAddressablesLocalBuildDir()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var profileSettings = settings.profileSettings;
            var propName = profileSettings.GetValueByName(settings.activeProfileId, "LocalBuildPath");
            return profileSettings.EvaluateString(settings.activeProfileId, propName);
        }

        private static void CacheRemoteAddressablesBundleList(AddressablesPlayerBuildResult result)
        {
            var buildRootDir = GetAddressablesRemoteBuildDir();
            var buildRootDirLen = buildRootDir.Length;
            CachedAssetBundles cachedBundles = new CachedAssetBundles();

            var filePathList = result.FileRegistry.GetFilePaths().Where(s => s.EndsWith(".bundle"));
            foreach (var filePath in filePathList)
            {
                var bundlePath = filePath.Substring(buildRootDirLen + 1);
                cachedBundles.cachedBundleList.Add(bundlePath);
            }

            if (!Directory.Exists(buildRootDir))
            {
                Directory.CreateDirectory(buildRootDir);
            }

            var json = JsonUtility.ToJson(cachedBundles);
            File.WriteAllText($"{buildRootDir}/CachedAssetBundles.json", json);
        }

        private static void BundleRemoteAddressables()
        {
            var remoteBuildDir = GetAddressablesRemoteBuildDir();
            var aaDestDir = GetAddressablesLocalBuildDir();

            if (!Directory.Exists(aaDestDir))
            {
                Directory.CreateDirectory(aaDestDir);
            }

            // Copy bundles to aa folder
            foreach (var srcFile in Directory.EnumerateFiles(remoteBuildDir, "*.*", SearchOption.AllDirectories))
            {
                var fileName = Path.GetFileName(srcFile);
                var destFile = $"{aaDestDir}/{fileName}";
                File.Copy(srcFile, destFile, true);
            }
        }

        #endregion
    }
}
