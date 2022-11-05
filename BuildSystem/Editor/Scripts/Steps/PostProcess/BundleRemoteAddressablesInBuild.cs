using Celeste.BuildSystem;
using CelesteEditor.Assets.Schemas;
using System.Collections.Generic;
using System.IO;
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
            HashSet<string> bundledNames = GetBundledAssetBundleNames();

            if (bundledNames.Count > 0)
            {
                CachedAssetBundles cachedAssetBundles = CacheRemoteAddressablesBundleList(result, bundledNames);
                BundleRemoteAddressables(cachedAssetBundles);
            }
        }

        #region Utility

        private static string GetAddressablesRemoteBuildDir()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var profileSettings = settings.profileSettings;
            var propName = profileSettings.GetValueByName(settings.activeProfileId, "RemoteBuildPath");
            return profileSettings.EvaluateString(settings.activeProfileId, propName);
        }

        private static string GetAddressablesLocalBuildDir()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var profileSettings = settings.profileSettings;
            var propName = profileSettings.GetValueByName(settings.activeProfileId, "LocalBuildPath");
            return profileSettings.EvaluateString(settings.activeProfileId, propName);
        }

        private static CachedAssetBundles CacheRemoteAddressablesBundleList(AddressablesPlayerBuildResult result, HashSet<string> assetBundleNames)
        {
            var buildRootDir = GetAddressablesRemoteBuildDir();
            var buildRootDirLen = buildRootDir.Length;
            CachedAssetBundles cachedBundles = new CachedAssetBundles();

            foreach (var assetBundleName in assetBundleNames)
            {
                string bundlePath = result.FileRegistry.GetFilePathForBundle(assetBundleName);

                // This string could be empty - it simply means one of the two bundles for a group wasn't present
                if (!string.IsNullOrEmpty(bundlePath))
                {
                    bundlePath = bundlePath.Substring(buildRootDirLen + 1);
                    cachedBundles.cachedBundleList.Add(bundlePath);
                }
            }

            if (!Directory.Exists(buildRootDir))
            {
                Directory.CreateDirectory(buildRootDir);
            }

            var json = JsonUtility.ToJson(cachedBundles);
            File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "CachedAssetBundles.json"), json);

            return cachedBundles;
        }

        private static void BundleRemoteAddressables(CachedAssetBundles cachedAssetBundles)
        {
            var remoteBuildDir = GetAddressablesRemoteBuildDir();
            var aaDestDir = GetAddressablesLocalBuildDir();

            if (!Directory.Exists(aaDestDir))
            {
                Directory.CreateDirectory(aaDestDir);
            }

            // Copy bundles to aa folder
            foreach (string relativeBundlePath in cachedAssetBundles.cachedBundleList)
            {
                var srcFile = Path.Combine(remoteBuildDir, relativeBundlePath);
                var destFile = Path.Combine(aaDestDir, relativeBundlePath);
                File.Copy(srcFile, destFile, true);
            }
        }

        private static HashSet<string> GetBundledAssetBundleNames()
        {
            HashSet<string> bundledAssetBundleNames = new HashSet<string>();
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            foreach (var group in settings.groups)
            {
                if (group.HasSchema<BundledGroupSchema>() &&
                    group.GetSchema<BundledGroupSchema>().BundleInStreamingAssets)
                {
                    string lowerCaseName = group.Name.ToLowerInvariant();

                    bundledAssetBundleNames.Add($"{lowerCaseName}_assets");
                    bundledAssetBundleNames.Add($"{lowerCaseName}_scenes");
                }
            }

            return bundledAssetBundleNames;
        }

        #endregion
    }
}
