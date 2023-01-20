using Celeste.Assets;
using Celeste.Components;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.LiveOps.Components
{
    [CreateAssetMenu(fileName = nameof(AddressableAssets), menuName = "Celeste/Live Ops/Assets/Addressables")]
    public class AddressableAssets : Celeste.Components.Component, ILiveOpAssets
    {
        #region Save Data

        [Serializable]
        public class AddressableAssetsData : ComponentData
        {
            public string label;
        }

        #endregion

        #region Properties and Fields

        public bool IsLoaded { get; private set; }

        #endregion

        public override ComponentData CreateData()
        {
            return new AddressableAssetsData();
        }

        public IEnumerator LoadAssets(Instance instance)
        {
            IsLoaded = false;

            AddressableAssetsData data = instance.data as AddressableAssetsData;
            var downloadSize = Addressables.GetDownloadSizeAsync(data.label);

            yield return downloadSize;

            if (downloadSize.Status != AsyncOperationStatus.Succeeded)
            {
                UnityEngine.Debug.LogError($"Failed to ascertain {data.label} addressables download size.  This may cause events to not run.");
                IsLoaded = false;
                yield break;
            }

            if (downloadSize.Result > 0)
            {
                var download = Addressables.DownloadDependenciesAsync(data.label);

                yield return download;

                if (download.Status != AsyncOperationStatus.Succeeded)
                {
                    UnityEngine.Debug.LogError($"Failed to download {data.label} addressables.  This may cause events to not run.");
                    IsLoaded = false;
                    yield break;
                }
            }

            IsLoaded = true;
        }

        public ILoadRequest<GameObject> InstantiateAsync(string key, Transform parent)
        {
            return LoadAddressableRequest<GameObject>.FromOperation(Addressables.InstantiateAsync(key, parent: parent, trackHandle: false));
        }
    }
}
