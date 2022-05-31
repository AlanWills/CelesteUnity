using Celeste.Assets;
using Celeste.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.LiveOps.Components
{
    [CreateAssetMenu(fileName = nameof(BakedAssets), menuName = "Celeste/Live Ops/Assets/Baked")]
    public class BakedAssets : Celeste.Components.Component, ILiveOpAssets
    {
        #region Baked Asset

        [Serializable]
        private struct BakedPrefab
        {
            public string key;
            public GameObject prefab;
        }

        #endregion

        #region Properties and Fields

        public bool IsLoaded => true;

        [SerializeField] private List<BakedPrefab> prefabs = new List<BakedPrefab>();

        #endregion

        public IEnumerator Load(Instance instance)
        {
            yield break;
        }

        public ILoadRequest<GameObject> InstantiateAsync(string key, Transform parent)
        {
            GameObject foundAsset = prefabs.Find(x => string.CompareOrdinal(key, x.key) == 0).prefab;
            GameObject foundAssetDuplicate = Instantiate(foundAsset, parent);

            return LoadAssetRequest<GameObject>.FromAsset(foundAssetDuplicate);
        }
    }
}
