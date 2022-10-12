using Celeste.BuildSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(LoadContentCatalogueLoadJob), menuName = "Celeste/Loading/Load Jobs/Load Content Catalogue")]
    public class LoadContentCatalogueLoadJob : LoadJob
    {
        #region Properties and Fields

        private List<string> _bundleCacheList = new List<string>();

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            yield return Addressables.InitializeAsync(true);

            string runtimeBuildSettingsFilePath = Path.Combine(Application.streamingAssetsPath, "BuildSettings.json");
            var runtimeBuildSettingsRequest = UnityWebRequest.Get(runtimeBuildSettingsFilePath);

            yield return runtimeBuildSettingsRequest.SendWebRequest();

            if (runtimeBuildSettingsRequest.result == UnityWebRequest.Result.Success)
            {
                RuntimeBuildSettings runtimeBuildSettings = JsonUtility.FromJson<RuntimeBuildSettings>(runtimeBuildSettingsRequest.downloadHandler.text);
                var loadCatalogue = Addressables.LoadContentCatalogAsync(runtimeBuildSettings.RemoteContentCatalogueJsonURL);

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
            else
            {
                Debug.LogError($"Could not load runtime build settings from streaming assets: {runtimeBuildSettingsFilePath}.");
            }
        }
    }
}