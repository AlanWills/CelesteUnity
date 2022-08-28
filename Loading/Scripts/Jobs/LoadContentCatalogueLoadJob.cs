using Celeste.Assets;
using Celeste.Log;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

        [Tooltip("Download the content catalogue at this URL.")]
        [SerializeField] private string contentCatalogueURL;

        private List<string> _bundleCacheList = new List<string>();

        #endregion

        //public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        //{
        //    var loadCatalogue = Addressables.LoadContentCatalogAsync(contentCatalogueURL);

        //    yield return loadCatalogue;
        //    yield return Addressables.UpdateCatalogs();

        //    if (loadCatalogue.Status == AsyncOperationStatus.Succeeded)
        //    {
        //        Addressables.ClearResourceLocators();
        //        Addressables.AddResourceLocator(loadCatalogue.Result);
        //    }
        //    else
        //    {
        //        yield return new WaitForSeconds(10);
        //    }

        //    Addressables.Release(loadCatalogue);
        //}

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            Debug.Log(Application.persistentDataPath);
            yield return Addressables.InitializeAsync();
            //var checkHandle = Addressables.CheckForCatalogUpdates(false);
            //yield return checkHandle;
            //if (checkHandle.Status == AsyncOperationStatus.Succeeded && checkHandle.Result.Count > 0)
            {
                //yield return Addressables.UpdateCatalogs(/*checkHandle.Result*/);
            }

            //Addressables.Release(checkHandle);

            //Get bundle list file from StreamingAsset
            var bundleCacheFileURL = $"{Addressables.RuntimePath}/{ToBuildPlatformString(Application.platform)}/CachedAssetBundles.json";
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        var url = bundleCacheFileURL;
#else
            var url = Path.GetFullPath(bundleCacheFileURL);
#endif
            var request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError(request.error);
            }

            Debug.Log($"BundleCache:{request.downloadHandler.text}");
            _bundleCacheList = JsonConvert.DeserializeObject<List<string>>(request.downloadHandler.text);
            Addressables.InternalIdTransformFunc = Addressables_InternalIdTransformFunc;
        }

        private string Addressables_InternalIdTransformFunc(IResourceLocation location)
        {
            if (location.Data is AssetBundleRequestOptions)
            {
                if (_bundleCacheList.Contains(location.PrimaryKey))
                {
                    var fileName = Path.GetFileName(location.PrimaryKey);
                    //Use LogError to test whether the StreamingAsset cache is used
                    HudLog.LogError($"StreamingAssetCache:{location.PrimaryKey}");
                    return $"{Addressables.RuntimePath}/{ToBuildPlatformString(Application.platform)}/{fileName}";
                }
                else
                {
                    HudLog.LogWarning($"{location.PrimaryKey} not {_bundleCacheList[0]}");
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
                    return "Windows";

                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";

                default:
                    return "Unknown";
            }
        }
    }
}