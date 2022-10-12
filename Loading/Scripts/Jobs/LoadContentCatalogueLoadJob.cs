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

        private string LocalContentCatalogueHashPath => Path.Combine(Application.persistentDataPath, "LocalContentCatalogue.hash");
        private string LocalContentCatalogueJsonPath => Path.Combine(Application.persistentDataPath, "LocalContentCatalogue.json");

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

                // Get the hash of the remote catalogue and compare it with what we have previously downloaded (if anything)
                // If the hash is different, download the new remote catalogue, then replace our local one with that
                // Then, load the new catalogue from the local path

                var remoteContentCatalogueHashRequest = UnityWebRequest.Get(runtimeBuildSettings.RemoteContentCatalogueHashURL);
                yield return remoteContentCatalogueHashRequest.SendWebRequest();

                if (remoteContentCatalogueHashRequest.result == UnityWebRequest.Result.Success)
                {
                    string localContentCatalogueHash = File.Exists(LocalContentCatalogueHashPath) ? File.ReadAllText(LocalContentCatalogueHashPath) : string.Empty;
                    string remoteContentCatalogueHash = remoteContentCatalogueHashRequest.downloadHandler.text;

                    // We have a remote content catalogue and it has a different hash to our local saved one
                    // We can download the new local content catalogue
                    if (!string.IsNullOrEmpty(remoteContentCatalogueHash) &&
                        string.CompareOrdinal(remoteContentCatalogueHash, localContentCatalogueHash) != 0)
                    {
                        var remoteContentCatalogueJsonRequest = UnityWebRequest.Get(runtimeBuildSettings.RemoteContentCatalogueJsonURL);
                        yield return remoteContentCatalogueJsonRequest.SendWebRequest();

                        if (remoteContentCatalogueJsonRequest.result == UnityWebRequest.Result.Success)
                        {
                            File.WriteAllText(LocalContentCatalogueJsonPath, remoteContentCatalogueJsonRequest.downloadHandler.text);
                        }
                        else
                        {
                            Debug.LogWarning($"Could not download remote catalogue json at URL: {runtimeBuildSettings.RemoteContentCatalogueJsonURL}.");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"Could not download remote catalogue hash at URL: {runtimeBuildSettings.RemoteContentCatalogueHashURL}.");
                }
            }
            else
            {
                Debug.LogError($"Could not load runtime build settings from streaming assets: {runtimeBuildSettingsFilePath}.");
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
        }
    }
}