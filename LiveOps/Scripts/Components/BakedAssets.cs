using Celeste.Assets;
using Celeste.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.LiveOps.Components
{
    [CreateAssetMenu(fileName = nameof(BakedAssets), menuName = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM + "Assets/Baked", order = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM_PRIORITY)]
    public class BakedAssets : BaseComponent, ILiveOpAssets
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

        public IEnumerator LoadAssets(Instance instance)
        {
            yield break;
        }

        public ILoadRequest<GameObject> LoadAsync(string key)
        {
            GameObject foundAsset = prefabs.Find(x => string.CompareOrdinal(key, x.key) == 0).prefab;
            UnityEngine.Debug.Assert(foundAsset != null, $"Could not find baked asset with key {key}.");
            return LoadAssetRequest<GameObject>.FromAsset(foundAsset);
        }

        public ILoadRequest<GameObject> InstantiateAsync(string key, Transform parent)
        {
            GameObject foundAsset = prefabs.Find(x => string.CompareOrdinal(key, x.key) == 0).prefab;
            UnityEngine.Debug.Assert(foundAsset != null, $"Could not find baked asset with key {key}.");
            GameObject foundAssetDuplicate = Instantiate(foundAsset, parent);

            return LoadAssetRequest<GameObject>.FromAsset(foundAssetDuplicate);
        }
    }
}
