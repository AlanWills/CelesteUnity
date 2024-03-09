using Celeste;
using Celeste.BuildSystem;
using CelesteEditor.Assets.Schemas;
using CelesteEditor.Tools;
using System.Collections.Generic;
using System.IO;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;
using UnityEngine.AddressableAssets.Initialization;

namespace CelesteEditor.BuildSystem.Steps
{
    [CreateAssetMenu(
        fileName = nameof(BundleRemoteAddressablesInBuild), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Asset Post Process/Bundle Remote Addressables In Build",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class BundleRemoteAddressablesInBuild : AssetPostProcessStep
    {
        #region Properties and Fields

        [SerializeField] private string unityBuiltInShadersPrefix = "alwaysbundle";

        #endregion

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

        private static CachedAssetBundles CacheRemoteAddressablesBundleList(AddressablesPlayerBuildResult result, HashSet<string> assetBundleNames)
        {
            var buildRootDir = AddressablesUtility.GetAddressablesRemoteBuildPath();
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

            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }

            var json = JsonUtility.ToJson(cachedBundles);
            File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "CachedAssetBundles.json"), json);

            return cachedBundles;
        }

        private static void BundleRemoteAddressables(CachedAssetBundles cachedAssetBundles)
        {
            var remoteBuildDir = AddressablesUtility.GetAddressablesRemoteBuildPath();
            var aaDestDir = AddressablesUtility.GetAddressablesLocalBuildPath();

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

            AddressablesRuntimeProperties.SetPropertyValue("Local.LoadPath", AddressablesUtility.GetAddressablesLocalLoadPath());
        }

        private HashSet<string> GetBundledAssetBundleNames()
        {
            HashSet<string> bundledAssetBundleNames = new HashSet<string>()
            {
                $"{unityBuiltInShadersPrefix}_unitybuiltinshaders"
            };
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
