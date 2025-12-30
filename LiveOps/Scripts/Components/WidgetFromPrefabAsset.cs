using Celeste.Assets;
using Celeste.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.LiveOps
{
    [CreateAssetMenu(fileName = nameof(WidgetFromPrefabAsset), menuName = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM + "Widget/From Prefab Asset", order = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM_PRIORITY)]
    public class WidgetFromPrefabAsset : Celeste.Components.BaseComponent, ILiveOpWidget
    {
        #region Save Data

        [Serializable]
        public class WidgetFromPrefabAssetData : ComponentData
        {
            public string PrefabKey;
            public List<LiveOpState> StatesToShow = new();
        }

        #endregion

        public override ComponentData CreateData()
        {
            return new WidgetFromPrefabAssetData();
        }

        public bool ShouldSpawnWidget(Instance instance, LiveOpState state)
        {
            return (instance.data as WidgetFromPrefabAssetData).StatesToShow.Contains(state);
        }

        public ILoadRequest<GameObject> SpawnWidget(Instance instance, ILiveOpAssets assets, Transform parent)
        {
            WidgetFromPrefabAssetData data = instance.data as WidgetFromPrefabAssetData;
            
            return assets.InstantiateAsync(data.PrefabKey, parent);
        }
    }
}
