using Celeste;
using Celeste.BuildSystem;
using CelesteEditor.Assets.Schemas;
using CelesteEditor.Tools;
using System.Collections.Generic;
using System.IO;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    [CreateAssetMenu(
        fileName = nameof(BundleRemoteAddressablesInBuild), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Asset Post Process/Bundle Remote Addressables In Build",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
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

        private static CachedAssetBundles CacheRemoteAddressablesBundleList(AddressablesPlayerBuildResult result, HashSet<string> assetBundleNames)
        {
            var buildRootDir = AddressablesUtility.GetAddressablesRemoteBuildPath();
            var buildRootDirLen = buildRootDir.Length;
            string localBuildPath = AddressablesUtility.GetAddressablesLocalBuildPath();
            string localLoadPath = AddressablesUtility.GetAddressablesLocalLoadPath();
            
            CachedAssetBundles cachedBundles = new CachedAssetBundles
            {
                cacheLocation = localLoadPath
            };

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

            if (!Directory.Exists(localBuildPath))
            {
                Directory.CreateDirectory(localBuildPath);
            }

            var json = JsonUtility.ToJson(cachedBundles);
            File.WriteAllText(Path.Combine(localBuildPath, "CachedAssetBundles.json"), json);

            return cachedBundles;
        }

        private static void BundleRemoteAddressables(CachedAssetBundles cachedAssetBundles)
        {
            var remoteBuildDir = AddressablesUtility.GetAddressablesRemoteBuildPath();
            var localBuildDir = AddressablesUtility.GetAddressablesLocalBuildPath();

            if (!Directory.Exists(localBuildDir))
            {
                Debug.Log($"Creating cached asset bundle location: {localBuildDir}.");
                Directory.CreateDirectory(localBuildDir);
            }

            // Copy bundles to Unity's local addressables build folder
            foreach (string relativeBundlePath in cachedAssetBundles.cachedBundleList)
            {
                var srcFile = Path.Combine(remoteBuildDir, relativeBundlePath);
                var destFile = Path.Combine(localBuildDir, relativeBundlePath);
                Debug.Log($"Moving source addressable asset bundle '{srcFile}' to '{destFile}'.");
                File.Copy(srcFile, destFile, true);
            }
        }

        private HashSet<string> GetBundledAssetBundleNames()
        {
            HashSet<string> bundledAssetBundleNames = new HashSet<string>();
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            
            if (settings.ShaderBundleNaming == ShaderBundleNaming.Custom)
            {
                bundledAssetBundleNames.Add($"{settings.ShaderBundleCustomNaming}_unitybuiltinshaders");
            }
            else
            {
                Debug.LogWarning($"Shader Bundle Naming is not set to custom in Addressable Settings.  This is likely incorrect and may lead to caching not working as intended.");
            }

            if (settings.MonoScriptBundleNaming == MonoScriptBundleNaming.Custom)
            {
                bundledAssetBundleNames.Add($"{settings.MonoScriptBundleCustomNaming}_monoscripts");
            }
            else if (settings.MonoScriptBundleNaming != MonoScriptBundleNaming.Disabled)
            {
                Debug.LogWarning($"Mono Script Bundle Naming is enabled, but not set to custom in Addressable Settings.  This is likely incorrect and may lead to caching not working as intended.");
            }

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
