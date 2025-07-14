#if USE_ADDRESSABLES
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
            if (!string.IsNullOrEmpty(result.Error))
            {
                // If something went wrong with the addressables build we should try and execute this step
                return;
            }
            
            HashSet<string> bundledNames = GetBundledAssetBundleNames(result);

            if (bundledNames.Count > 0)
            {
                CachedAssetBundles cachedAssetBundles = CacheRemoteAddressablesBundleList(result, bundledNames);
                BundleRemoteAddressables(cachedAssetBundles);
            }
        }

        #region Utility

        private static CachedAssetBundles CacheRemoteAddressablesBundleList(AddressablesPlayerBuildResult result, HashSet<string> assetBundleNames)
        {
            var buildRootDir = AddressablesExtensions.GetAddressablesRemoteBuildPath();
            var buildRootDirLen = buildRootDir.Length;
            string localBuildPath = AddressablesExtensions.GetAddressablesLocalBuildPath();
            string localLoadPath = AddressablesExtensions.GetAddressablesLocalLoadPath();
            
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
            var remoteBuildDir = AddressablesExtensions.GetAddressablesRemoteBuildPath();
            var localBuildDir = AddressablesExtensions.GetAddressablesLocalBuildPath();

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

        private HashSet<string> GetBundledAssetBundleNames(AddressablesPlayerBuildResult result)
        {
            HashSet<string> bundledAssetBundleNames = new HashSet<string>();
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            var localBuildDir = AddressablesExtensions.GetAddressablesLocalBuildPath();

            // Unity Built In Shaders
            {
                string unityBuiltInShadersName = $"{settings.BuiltInBundleCustomNaming}_unitybuiltinshaders";
                string unityBuiltInShadersPath = result.FileRegistry.GetFilePathForBundle(unityBuiltInShadersName);

                if (!string.IsNullOrEmpty(unityBuiltInShadersPath) && !unityBuiltInShadersPath.StartsWith(localBuildDir))
                {
                    if (settings.BuiltInBundleNaming == BuiltInBundleNaming.Custom)
                    {
                        bundledAssetBundleNames.Add(unityBuiltInShadersName);
                    }
                    else
                    {
                        Debug.LogWarning(
                            $"Unity Build In Shader Bundle Naming is not set to custom in Addressable Settings.  This is likely incorrect and may lead to caching not working as intended.");
                    }
                }
            }

            // Unity Built In Assets
            {
                string unityBuiltInAssetsName = $"{settings.BuiltInBundleCustomNaming}_unitybuiltinassets";
                string unityBuiltInAssetsPath = result.FileRegistry.GetFilePathForBundle(unityBuiltInAssetsName);

                if (!string.IsNullOrEmpty(unityBuiltInAssetsPath) && !unityBuiltInAssetsPath.StartsWith(localBuildDir))
                {
                    if (settings.BuiltInBundleNaming == BuiltInBundleNaming.Custom)
                    {
                        bundledAssetBundleNames.Add(unityBuiltInAssetsName);
                    }
                    else
                    {
                        Debug.LogWarning(
                            $"Unity Built In Assets Bundle Naming is not set to custom in Addressable Settings.  This is likely incorrect and may lead to caching not working as intended.");
                    }
                }
            }

            // Monoscript Bundle
            {
                string monoscriptBundleName = $"{settings.MonoScriptBundleCustomNaming}_monoscripts";
                string monoscriptBundlePath = result.FileRegistry.GetFilePathForBundle(monoscriptBundleName);

                if (!string.IsNullOrEmpty(monoscriptBundlePath) && !monoscriptBundlePath.StartsWith(localBuildDir))
                {
                    if (settings.MonoScriptBundleNaming == MonoScriptBundleNaming.Custom)
                    {
                        bundledAssetBundleNames.Add($"{settings.MonoScriptBundleCustomNaming}_monoscripts");
                    }
                    else
                    {
                        Debug.LogWarning(
                            $"Mono Script Bundle Naming is enabled, but not set to custom in Addressable Settings.  This is likely incorrect and may lead to caching not working as intended.");
                    }
                }
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
#endif