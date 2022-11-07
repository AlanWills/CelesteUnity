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
    [CreateAssetMenu(fileName = nameof(EnableBundledAddressablesLoadJob), menuName = "Celeste/Loading/Load Jobs/Enable Bundled Addressables")]
    public class EnableBundledAddressablesLoadJob : LoadJob
    {
        #region Properties and Fields

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
            var loadBundleCacheRequest = UnityWebRequest.Get(url);
            yield return loadBundleCacheRequest.SendWebRequest();

            if (!string.IsNullOrEmpty(loadBundleCacheRequest.error))
            {
                Debug.LogError(loadBundleCacheRequest.error);
            }

            JsonUtility.FromJsonOverwrite(loadBundleCacheRequest.downloadHandler.text, cachedAssetBundles);

            if (cachedAssetBundles != null && cachedAssetBundles.IsValid)
            {
                Addressables.InternalIdTransformFunc = Addressables_InternalIdTransformFunc;
            }
            else
            {
                Debug.LogError($"No cached asset bundles found - ignoring custom transform func.");
            }
        }

        private string Addressables_InternalIdTransformFunc(IResourceLocation location)
        {
            if (location.Data is AssetBundleRequestOptions)
            {
                if (cachedAssetBundles.cachedBundleList.Contains(location.PrimaryKey))
                {
                    var fileName = Path.GetFileName(location.PrimaryKey);
                    return $"{Addressables.RuntimePath}/{fileName}";
                }
            }

            return location.InternalId;
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