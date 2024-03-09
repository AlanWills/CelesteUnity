using Celeste.BuildSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(EnableBundledAddressablesLoadJob), menuName = CelesteMenuItemConstants.LOADING_MENU_ITEM + "Load Jobs/Enable Bundled Addressables", order = CelesteMenuItemConstants.LOADING_MENU_ITEM_PRIORITY)]
    public class EnableBundledAddressablesLoadJob : LoadJob
    {
        #region Properties and Fields

        private RuntimePlatform RuntimePlatform
        {
            get
            {
#if UNITY_EDITOR
    #if UNITY_ANDROID
                return RuntimePlatform.Android;
    #elif UNITY_IOS
                return RuntimePlatform.IPhonePlayer;
    #elif UNITY_STANDALONE_OSX
                return RuntimePlatform.OSXPlayer;
    #elif UNITY_STANDALONE_WIN
                return RuntimePlatform.WindowsPlayer;
    #endif
#else
                return Application.platform;
#endif
            }
        }

        private CachedAssetBundles cachedAssetBundles = new CachedAssetBundles();

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            // Get bundle list file from StreamingAsset
            var bundleCacheFileURL = Path.Combine(Application.streamingAssetsPath, "CachedAssetBundles.json");
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
                    string localLoadPath = GetAddressablesLocalLoadPath();
                    return $"{localLoadPath}/{fileName}";
                }
            }

            return location.InternalId;
        }

        private static string GetAddressablesLocalLoadPath()
        {
#if UNITY_EDITOR
            return CelesteEditor.Tools.AddressablesUtility.GetAddressablesLocalBuildPath();
#else
            return Addressables.RuntimePath;
#endif
        }

        private static string ToBuildPlatformString(RuntimePlatform runtimePlatform)
        {
            switch (runtimePlatform)
            {
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";

                case RuntimePlatform.Android:
                    return "Android";

                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return "StandaloneWindows64";

                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";

                default:
                    return "Unknown";
            }
        }
    }
}