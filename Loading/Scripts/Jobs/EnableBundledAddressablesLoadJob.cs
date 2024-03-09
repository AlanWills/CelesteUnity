using Celeste.BuildSystem;
using CelesteEditor.Tools;
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

        private CachedAssetBundles cachedAssetBundles = new CachedAssetBundles();

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            foreach (var file in Directory.GetFiles(Application.streamingAssetsPath, "*", SearchOption.AllDirectories))
            {
                Debug.Log($"Found file: {file} in {Application.streamingAssetsPath}.");
            }

            var bundleCacheFileURL = GetCachedAssetBundlesPath();
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
                Debug.Log($"Bundle cache info loaded successfully.");
                JsonUtility.FromJsonOverwrite(loadBundleCacheRequest.downloadHandler.text, cachedAssetBundles);
            }

            if (cachedAssetBundles != null && cachedAssetBundles.IsValid)
            {
                string addressablesLocalLoadPath = GetAddressablesLocalLoadPath(cachedAssetBundles);
                Debug.Log($"Addressables Local Load Path is: {addressablesLocalLoadPath}");

                foreach (var file in Directory.GetFiles(addressablesLocalLoadPath))
                {
                    Debug.Log($"Found file: {file} in Addressables Local Load Path.");
                }

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
                if (cachedAssetBundles.cachedBundleList.Contains(location.PrimaryKey))
                {
                    var fileName = Path.GetFileName(location.PrimaryKey);
                    string localLoadPath = GetAddressablesLocalLoadPath(cachedAssetBundles);
                    return $"{localLoadPath}/{fileName}";
                }
            }

            return location.InternalId;
        }

        private static string GetCachedAssetBundlesPath()
        {
#if UNITY_EDITOR
            return Path.Combine(AddressablesUtility.GetAddressablesLocalBuildPath(), "CachedAssetBundles.json");
#else
            return Path.Combine(Application.streamingAssetsPath, "CachedAssetBundles.json");
#endif
        }

        private static string GetAddressablesLocalLoadPath(CachedAssetBundles cachedAssetBundles)
        {
            return AddressablesRuntimeProperties.EvaluateString(cachedAssetBundles.cacheLocation);
        }
    }
}