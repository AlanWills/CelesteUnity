#if USE_ADDRESSABLES
using Celeste.BuildSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.Initialization;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(EnableBundledAddressablesLoadJob), menuName = CelesteMenuItemConstants.LOADING_MENU_ITEM + "Load Jobs/Enable Bundled Addressables", order = CelesteMenuItemConstants.LOADING_MENU_ITEM_PRIORITY)]
    public class EnableBundledAddressablesLoadJob : LoadJob
    {
        #region Runtime Platform Directory

        [Serializable]
        private struct RuntimePlatformDirectory
        {
            public RuntimePlatform platform;
            public string directory;

            public RuntimePlatformDirectory(RuntimePlatform platform, string directory)
            {
                this.platform = platform;
                this.directory = directory;
            }
        }

        #endregion

        #region Properties and Fields

        [Tooltip("If enabled, we will attempt to match a cached asset bundle against only the start of a requested bundle.  This can be useful if a requested bundle has a hash appended, but you have just cached the name.")]
        [SerializeField] private bool fuzzyNameMatching = true;
        [SerializeField] private List<RuntimePlatformDirectory> runtimePlatformDirectoryLookup = new List<RuntimePlatformDirectory>()
        {
            new RuntimePlatformDirectory(RuntimePlatform.Android, "Android"),
            new RuntimePlatformDirectory(RuntimePlatform.IPhonePlayer, "iOS"),
            new RuntimePlatformDirectory(RuntimePlatform.WebGLPlayer, "WebGL"),
            new RuntimePlatformDirectory(RuntimePlatform.WindowsPlayer, "StandaloneWindows64"),
            new RuntimePlatformDirectory(RuntimePlatform.WindowsEditor, "StandaloneWindows64"),
            new RuntimePlatformDirectory(RuntimePlatform.OSXPlayer, "StandaloneOSX"),
            new RuntimePlatformDirectory(RuntimePlatform.OSXEditor, "StandaloneOSX"),
        };

        private CachedAssetBundles cachedAssetBundles = new CachedAssetBundles();

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            RuntimePlatform platform = Application.platform;
            Debug.Log($"Attempting to load cached asset bundle info for platform '{platform}'.");

            var entry = runtimePlatformDirectoryLookup.Find(x => x.platform == platform);
            Debug.Assert(!string.IsNullOrEmpty(entry.directory), $"Failed to find a pre-specified addressables sub-folder for platform '{platform}'.");

            string subFolder = string.IsNullOrEmpty(entry.directory) ? platform.ToString() : entry.directory;
            Debug.Log($"Using addressables sub-folder '{subFolder}'.");

            var bundleCacheFileURL = GetCachedAssetBundlesPath(subFolder);

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            var url = bundleCacheFileURL;
#else
            var url = Path.GetFullPath(bundleCacheFileURL);
#endif
            yield return LoadCachedBundlesInfo(url);

            if (cachedAssetBundles != null && cachedAssetBundles.IsValid)
            {
                string addressablesLocalLoadPath = GetAddressablesLocalLoadPath(cachedAssetBundles);
                Debug.Log($"Addressables Local Load Path is: {addressablesLocalLoadPath}");

                Addressables.InternalIdTransformFunc = Addressables_InternalIdTransformFunc;
            }
            else
            {
                Debug.LogWarning($"No cached asset bundles found - ignoring custom transform func.");
            }
        }

        private IEnumerator LoadCachedBundlesInfo(string url)
        {
            Debug.Log($"Beginning to load bundle cache info at {url}.");
            string cachedBundlesInfo = "";

#if UNITY_ANDROID && !UNITY_EDITOR
            var loadBundleCacheRequest = UnityWebRequest.Get(url);
            yield return loadBundleCacheRequest.SendWebRequest();

            if (!string.IsNullOrEmpty(loadBundleCacheRequest.error))
            {
                Debug.LogError(loadBundleCacheRequest.error);
            }
            else
            {
                cachedBundlesInfo = loadBundleCacheRequest.downloadHandler.text;
            }
#else
            if (File.Exists(url))
            {
                cachedBundlesInfo = File.ReadAllText(url);
            }
            else
            {
                Debug.LogError($"Failed to find the file for bundle cache info at path {url}.");
            }
#endif
            Debug.Assert(!string.IsNullOrEmpty(cachedBundlesInfo), "Cached Bundles Info is empty!");
            if (!string.IsNullOrEmpty(cachedBundlesInfo))
            {
                Debug.Log($"Bundled cache info loaded successfully {cachedBundlesInfo}.");
                JsonUtility.FromJsonOverwrite(cachedBundlesInfo, cachedAssetBundles);
            }

            yield break;
        }

        private string Addressables_InternalIdTransformFunc(IResourceLocation location)
        {
            if (location.Data is AssetBundleRequestOptions)
            {
                Debug.Log($"Addressables Asset Bundle {location.PrimaryKey} load attempt using cached asset bundles.");
                
                if (cachedAssetBundles.cachedBundleList.Contains(location.PrimaryKey))
                {
                    string localLoadPath = GetAddressablesLocalLoadPath(cachedAssetBundles);
                    return $"{localLoadPath}/{location.PrimaryKey}";
                }
                else if (fuzzyNameMatching)
                {
                    string primaryKeyWithoutExtension = RemoveExtension(location.PrimaryKey);
                    var cachedBundle = cachedAssetBundles.cachedBundleList.Find(x => primaryKeyWithoutExtension.StartsWith(RemoveExtension(x), StringComparison.Ordinal));

                    if (!string.IsNullOrEmpty(cachedBundle))
                    {
                        string localLoadPath = GetAddressablesLocalLoadPath(cachedAssetBundles);
                        return $"{localLoadPath}/{cachedBundle}";
                    }
                }

                Debug.Log($"Failed to load {location.PrimaryKey} from our asset bundle cache (fuzzy matching? {fuzzyNameMatching}).");
            }

            return location.InternalId;
        }

        private static string RemoveExtension(string path)
        {
            int indexOfFileExtension = path.IndexOf('.');
            return indexOfFileExtension >= 0 ? path.Substring(0, indexOfFileExtension) : path;
        }

        private static string GetCachedAssetBundlesPath(string addressablesRuntimePathSubFolder)
        {
#if UNITY_EDITOR
            // Need to do this to support using built addressables in Unity
            // Since stuff only gets copied to StreamingAssets during buildtime (plus it would be messy for source control)
            // we have to redirect where we get the CachedAssetBundles info from
            return Path.Combine(CelesteEditor.Tools.AddressablesExtensions.GetAddressablesLocalBuildPath(), "CachedAssetBundles.json");
#else
            return Path.Combine(Addressables.RuntimePath, addressablesRuntimePathSubFolder, "CachedAssetBundles.json");
#endif
        }

        private static string GetAddressablesLocalLoadPath(CachedAssetBundles cachedAssetBundles)
        {
            return AddressablesRuntimeProperties.EvaluateString(cachedAssetBundles.cacheLocation);
        }
    }
}
#endif