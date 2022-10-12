using Newtonsoft.Json;
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

        private List<string> _bundleCacheList = new List<string>();

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            // Get bundle list file from StreamingAsset
            var bundleCacheFileURL = $"{Addressables.RuntimePath}/{ToBuildPlatformString(Application.platform)}/CachedAssetBundles.json";
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

            _bundleCacheList = JsonConvert.DeserializeObject<List<string>>(loadBundleCacheRequest.downloadHandler.text);

            if (_bundleCacheList != null)
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
                if (_bundleCacheList.Contains(location.PrimaryKey))
                {
                    var fileName = Path.GetFileName(location.PrimaryKey);
                    return $"{Addressables.RuntimePath}/{ToBuildPlatformString(Application.platform)}/{fileName}";
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