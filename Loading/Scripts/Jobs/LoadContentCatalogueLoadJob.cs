using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(LoadContentCatalogueLoadJob), menuName = "Celeste/Loading/Load Jobs/Load Content Catalogue")]
    public class LoadContentCatalogueLoadJob : LoadJob
    {
        #region Properties and Fields

        private string LocalContentCatalogueHashPath => Path.Combine(Application.persistentDataPath, "LocalContentCatalogue.hash");
        private string LocalContentCatalogueJsonPath => Path.Combine(Application.persistentDataPath, "LocalContentCatalogue.json");

        [Tooltip("The URL to the file containing the hash of the latest content catalogue.")]
        [SerializeField] private string remoteContentCatalogueHashURL;

        [Tooltip("The URL to the file containing the json of the latest content catalogue.")]
        [SerializeField] private string remoteContentCatalogueJsonURL;

        private List<string> _bundleCacheList = new List<string>();

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            yield return Addressables.InitializeAsync(true);
            
            // Get the hash of the remote catalogue and compare it with what we have previously downloaded (if anything)
            // If the hash is different, download the new remote catalogue, then replace our local one with that
            // Then, load the new catalogue from the local path

            var remoteContentCatalogueHashRequest = UnityWebRequest.Get(remoteContentCatalogueHashURL);
            yield return remoteContentCatalogueHashRequest.SendWebRequest();

            if (remoteContentCatalogueHashRequest.result == UnityWebRequest.Result.Success)
            {
                string localContentCatalogueHash = File.Exists(LocalContentCatalogueHashPath) ? File.ReadAllText(LocalContentCatalogueHashPath) : string.Empty;
                string remoteContentCatalogueHash = remoteContentCatalogueHashRequest.downloadHandler.text;

                if (!string.IsNullOrEmpty(remoteContentCatalogueHash))
                {
                    var remoteContentCatalogueJsonRequest = UnityWebRequest.Get(remoteContentCatalogueJsonURL);
                    yield return remoteContentCatalogueJsonRequest.SendWebRequest();

                    if (remoteContentCatalogueJsonRequest.result == UnityWebRequest.Result.Success)
                    {
                        File.WriteAllText(LocalContentCatalogueJsonPath, remoteContentCatalogueJsonRequest.downloadHandler.text);
                    }
                    else
                    {
                        Debug.LogWarning($"Could not download remote catalogue json at URL: {remoteContentCatalogueJsonURL}.");
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Could not download remote catalogue hash at URL: {remoteContentCatalogueHashURL}.");
            }

            if (File.Exists(LocalContentCatalogueJsonPath))
            {
                Debug.Assert(!string.IsNullOrEmpty(File.ReadAllText(LocalContentCatalogueJsonPath)), $"Found an empty local content catalogue at: {LocalContentCatalogueJsonPath}.");
                var loadCatalogue = Addressables.LoadContentCatalogAsync(LocalContentCatalogueJsonPath);

                yield return loadCatalogue;

                if (loadCatalogue.Status == AsyncOperationStatus.Succeeded)
                {
                    // We have to do this so that our remote content catalogue is prioritised over local catalogues
                    // As an example of why this is required, go to AddressablesImpl.LoadSceneAsync
                    // You will see we gather the resource locations and use the first one
                    // However, unless our catalogue resource locator is first, we will use the local address not the remote address
                    List<IResourceLocator> resourceLocators = new List<IResourceLocator>(Addressables.ResourceLocators);

                    foreach (var resourceLocator in resourceLocators)
                    {
                        Addressables.RemoveResourceLocator(resourceLocator);
                    }

                    Addressables.AddResourceLocator(loadCatalogue.Result);

                    foreach (var resourceLocator in resourceLocators)
                    {
                        Addressables.AddResourceLocator(resourceLocator);
                    }
                }

                Addressables.Release(loadCatalogue);
            }

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