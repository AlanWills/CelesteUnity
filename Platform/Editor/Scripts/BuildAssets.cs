using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityEngine.Application;

namespace CelesteEditor.BuildSystem
{
    public static class BuildEditor
    {
        [MenuItem("Tools/Build Resource #%&t")]
        public static void BuildResource()
        {
            var buildRootDir = GetAASRemoteBuildDir();
            //Delete existing folder
            Directory.Delete(buildRootDir, true);
            //Execute packaging logic
            AddressableAssetSettings.BuildPlayerContent(out var result);
            BuildResource_CacheBundleList(result);
            BuildResource_CopyBundleDirToAAS();
            Debug.Log($"build resource finish");
        }

        /// <summary>
        ///Get RemoteBuild folder path
        /// </summary>
        /// <returns></returns>
        public static string GetAASRemoteBuildDir()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var profileSettings = settings.profileSettings;
            var propName =
                profileSettings.GetValueByName(settings.activeProfileId, "RemoteBuildPath");
            return propName;
        }

        /// <summary>
        ///Get LocalBuild folder path
        /// </summary>
        /// <returns></returns>
        public static string GetAASLocalBuildDir()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var profileSettings = settings.profileSettings;
            var propName =
                profileSettings.GetValueByName(settings.activeProfileId, "LocalBuildPath");
            return profileSettings.EvaluateString(settings.activeProfileId, propName);
        }

        /// <summary>
        ///Generate BundleCache file list
        /// </summary>
        /// <param name="result"></param>
        static void BuildResource_CacheBundleList(AddressablesPlayerBuildResult result)
        {
            var buildRootDir = GetAASRemoteBuildDir();
            var buildRootDirLen = buildRootDir.Length;
            List<string> allBundles = new List<string>();

            var filePathList = result.FileRegistry.GetFilePaths().Where((s => s.EndsWith(".bundle")));
            foreach (var filePath in filePathList)
            {
                var bundlePath = filePath.Substring(buildRootDirLen + 1);
                allBundles.Add(bundlePath);
            }

            var json = JsonConvert.SerializeObject(allBundles);
            File.WriteAllText($"{buildRootDir}/AssetBundles.json", json);
        }

        /// <summary>
        ///Copy the Bundle to AAS and package it with AAS
        /// </summary>
        static void BuildResource_CopyBundleDirToAAS()
        {
            var remoteBuildDir = GetAASRemoteBuildDir();
            var aaDestDir = GetAASLocalBuildDir();

            //Directory.Delete(aaDestDir, true);
            Directory.CreateDirectory(aaDestDir);
            //Copy bundle to aa folder
            var allSrcFile = Directory.EnumerateFiles(remoteBuildDir, "*.*", SearchOption.AllDirectories);
            foreach (var srcFile in allSrcFile)
            {
                var fileName = Path.GetFileName(srcFile);
                var destFile = $"{aaDestDir}/{fileName}";
                File.Copy(srcFile, destFile);
            }

            Debug.Log($"Bundle Copy complete:{remoteBuildDir}==>{aaDestDir}");
        }
    }
    public static class BuildAssets
    {
        #region Android

        [MenuItem("Celeste/Assets/Debug/Build Android Assets", validate = true)]
        public static bool ValidateBuildDebugAndroidAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().AndroidDebug != null;
        }

        [MenuItem("Celeste/Assets/Debug/Build Android Assets", validate = false)]
        public static void BuildDebugAndroidAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().AndroidDebug.BuildAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Build Android Assets", validate = true)]
        public static bool ValidateBuildReleaseAndroidAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().AndroidReleaseBundle != null;
        }

        [MenuItem("Celeste/Assets/Release/Build Android Assets", validate = false)]
        public static void BuildReleaseAndroidAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().AndroidReleaseBundle.BuildAssetsAndExit();
        }

        #endregion

        #region Windows

        [MenuItem("Celeste/Assets/Debug/Build Windows Assets", validate = true)]
        public static bool ValidateBuildDebugWindowsAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().WindowsDebug != null;
        }

        [MenuItem("Celeste/Assets/Debug/Build Windows Assets", validate = false)]
        public static void BuildDebugWindowsAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WindowsDebug.BuildAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Build Windows Assets", validate = true)]
        public static bool ValidateBuildReleaseWindowsAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().WindowsRelease != null;
        }

        [MenuItem("Celeste/Assets/Release/Build Windows Assets", validate = false)]
        public static void BuildReleaseWindowsAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WindowsRelease.BuildAssetsAndExit();
        }

        #endregion

        #region iOS

        [MenuItem("Celeste/Assets/Debug/Build iOS Assets", validate = true)]
        public static bool ValidateBuildDebugiOSAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().iOSDebug != null;
        }

        [MenuItem("Celeste/Assets/Debug/Build iOS Assets", validate = false)]
        public static void BuildDebugiOSAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().iOSDebug.BuildAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Build iOS Assets", validate = true)]
        public static bool ValidateBuildReleaseiOSAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().iOSRelease != null;
        }

        [MenuItem("Celeste/Assets/Release/Build iOS Assets", validate = false)]
        public static void BuildReleaseiOSAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().iOSRelease.BuildAssetsAndExit();
        }

        #endregion

        #region WebGL

        [MenuItem("Celeste/Assets/Debug/Build WebGL Assets", validate = true)]
        public static bool ValidateBuildDebugWebGLAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().WebGLDebug != null;
        }

        [MenuItem("Celeste/Assets/Debug/Build WebGL Assets", validate = false)]
        public static void BuildDebugWebGLAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WebGLDebug.BuildAssetsAndExit();
        }

        [MenuItem("Celeste/Assets/Release/Build WebGL Assets", validate = true)]
        public static bool ValidateBuildReleaseWebGLAssets()
        {
            return AllPlatformSettings.GetOrCreateSettings().WebGLRelease != null;
        }

        [MenuItem("Celeste/Assets/Release/Build WebGL Assets", validate = false)]
        public static void BuildReleaseWebGLAssets()
        {
            AllPlatformSettings.GetOrCreateSettings().WebGLRelease.BuildAssetsAndExit();
        }

        #endregion

        private static void BuildAssetsAndExit(this PlatformSettings platformSettings)
        {
            string exceptionMessage = "";
            bool hasThrownAnException = false;
            LogCallback logDelegate = (string logString, string stackTrace, LogType type) =>
            {
                if (type == LogType.Exception)
                {
                    hasThrownAnException = true;
                    exceptionMessage = logString;
                }
            };
            Application.logMessageReceived += logDelegate;
            platformSettings.BuildAssets();
            Application.logMessageReceived -= logDelegate;

            if (hasThrownAnException)
            {
                Debug.LogErrorFormat("Exception thrown during asset building: {0}", exceptionMessage);
            }

            if (Application.isBatchMode)
            {
                // 0 = everything OK
                // 1 = everything NOT OK
                EditorApplication.Exit(hasThrownAnException ? 1 : 0);
            }
            else
            {
                EditorUtility.DisplayDialog("Asset Build result", string.Format("Assets Built {0}", hasThrownAnException ? "Unsuccessfully" : "Successfully"), "OK");
            }
        }
    }
}
