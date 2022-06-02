using Celeste.Assets;
using Celeste.Components;
using System;
using UnityEngine;

namespace Celeste.LiveOps
{
    [CreateAssetMenu(fileName = nameof(UIFromPrefabAsset), menuName = "Celeste/Live Ops/UI/From Prefab Asset")]
    public class UIFromPrefabAsset : Celeste.Components.Component, ILiveOpUI
    {
        #region Save Data

        [Serializable]
        public class UIFromPrefabAssetData : ComponentData
        {
            public string prefabKey;
        }

        #endregion

        public override ComponentData CreateData()
        {
            return new UIFromPrefabAssetData();
        }

        public ILoadRequest<GameObject> LoadUI(Instance instance, ILiveOpAssets assets, Transform parent)
        {
            UIFromPrefabAssetData data = instance.data as UIFromPrefabAssetData;
            
            return assets.InstantiateAsync(data.prefabKey, parent);
        }
    }
}
