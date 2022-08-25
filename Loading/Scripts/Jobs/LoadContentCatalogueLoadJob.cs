using Celeste.Assets;
using Celeste.Log;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(LoadContentCatalogueLoadJob), menuName = "Celeste/Loading/Load Jobs/Load Content Catalogue")]
    public class LoadContentCatalogueLoadJob : LoadJob
    {
        #region Properties and Fields

        [Tooltip("Download the content catalogue at this URL.")]
        [SerializeField] private string contentCatalogueURL;

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            var loadCatalogue = Addressables.LoadContentCatalogAsync(contentCatalogueURL);
            
            yield return loadCatalogue;

            //Addressables.UpdateCatalogs()

            if (loadCatalogue.Status == AsyncOperationStatus.Succeeded)
            {
                Addressables.ClearResourceLocators();
                Addressables.AddResourceLocator(loadCatalogue.Result);
            }

            Addressables.Release(loadCatalogue);
        }
    }
}