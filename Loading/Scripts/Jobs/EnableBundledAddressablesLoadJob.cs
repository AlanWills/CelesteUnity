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
        #region Properties and Fields

        [SerializeField] private List<ValueTuple<RuntimePlatform, string>> runtimePlatformDirectoryLookup = new List<ValueTuple<RuntimePlatform, string>>()
        {
            new ValueTuple<RuntimePlatform, string>(RuntimePlatform.Android, "Android"),
            new ValueTuple<RuntimePlatform, string>(RuntimePlatform.IPhonePlayer, "iOS"),
            new ValueTuple<RuntimePlatform, string>(RuntimePlatform.WebGLPlayer, "WebGL"),
            new ValueTuple<RuntimePlatform, string>(RuntimePlatform.WindowsPlayer, "StandaloneWindows64"),
        };

        private CachedAssetBundles cachedAssetBundles = new CachedAssetBundles();

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            RuntimePlatform platform = Application.platform;
            Debug.Log($"Attempting to load cached asset bundle info for platform '{platform}'.");

            var entry = runtimePlatformDirectoryLookup.Find(x => x.Item1 == platform);
            Debug.Assert(!string.IsNullOrEmpty(entry.Item2), $"Failed to find a pre-specified addressables sub-folder for platform '{platform}'.");

            string subFolder = string.IsNullOrEmpty(entry.Item2) ? platform.ToString() : entry.Item2;
            Debug.Log($"Using addressables sub-folder '{subFolder}'.");

            var bundleCacheFileURL = GetCachedAssetBundlesPath(subFolder);

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            var url = bundleCacheFileURL;
#else
            var url = Path.GetFullPath(bundleCacheFileURL);
#endif
            Debug.Log($"Beginning to load bundle cache info at {url}.");
            var loadBundleCacheRequest = UnityWebRequest.Get(url);
            yield return loadBundleCacheRequest.SendWebRequest();

            if (!string.IsNullOrEmpty(loadBundleCacheRequest.error))
            {
                Debug.LogError(loadBundleCacheRequest.error);
            }
            else
            {
                Debug.Log($"Bundled cache info loaded successfully {loadBundleCacheRequest.downloadHandler.text}.");
                JsonUtility.FromJsonOverwrite(loadBundleCacheRequest.downloadHandler.text, cachedAssetBundles);
            }

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

        private string Addressables_InternalIdTransformFunc(IResourceLocation location)
        {
            if (location.Data is AssetBundleRequestOptions)
            {
                Debug.Log($"Addressables Asset Bundle {location.PrimaryKey} load attempt using cached asset bundles.");
                
                if (cachedAssetBundles.cachedBundleList.Contains(location.PrimaryKey))
                {
                    var fileName = Path.GetFileName(location.PrimaryKey);
                    string localLoadPath = GetAddressablesLocalLoadPath(cachedAssetBundles);
                    return $"{localLoadPath}/{fileName}";
                }
                else
                {
                    Debug.LogError($"Failed to load {location.PrimaryKey} from our asset bundle cache.");
                }
            }

            return location.InternalId;
        }

        private static string GetCachedAssetBundlesPath(string addressablesRuntimePathSubFolder)
        {
#if UNITY_EDITOR
            // Need to do this to support using built addressables in Unity
            // Since stuff only gets copied to StreamingAssets during buildtime (plus it would be messy for source control)
            // we have to redirect where we get the CachedAssetBundles info from
            return Path.Combine(CelesteEditor.Tools.AddressablesUtility.GetAddressablesLocalBuildPath(), "CachedAssetBundles.json");
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