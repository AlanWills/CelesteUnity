﻿using Celeste.Tools.Attributes.GUI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            List<string> allBundles = new List<string>();

            var filePathList = result.FileRegistry.GetFilePaths().Where(s => s.EndsWith(".bundle"));
            foreach (var filePath in filePathList)
            {
                var bundlePath = filePath.Substring(buildRootDirLen + 1);
                allBundles.Add(bundlePath);
            }

            if (!Directory.Exists(buildRootDir))
            {
                Directory.CreateDirectory(buildRootDir);
            }

            var json = JsonConvert.SerializeObject(allBundles);
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