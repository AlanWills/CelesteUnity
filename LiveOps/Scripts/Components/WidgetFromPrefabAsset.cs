﻿using Celeste.Assets;
using Celeste.Components;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.LiveOps
{
    [CreateAssetMenu(fileName = nameof(WidgetFromPrefabAsset), menuName = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM + "Widget/From Prefab Asset", order = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM_PRIORITY)]
    public class WidgetFromPrefabAsset : Celeste.Components.Component, ILiveOpWidget
    {
        #region Save Data

        [Serializable]
        public class WidgetFromPrefabAssetData : ComponentData
        {
            public string prefabKey;
        }

        #endregion

        public override ComponentData CreateData()
        {
            return new WidgetFromPrefabAssetData();
        }

        public ILoadRequest<GameObject> LoadWidget(Instance instance, ILiveOpAssets assets, Transform parent)
        {
            WidgetFromPrefabAssetData data = instance.data as WidgetFromPrefabAssetData;
            
            return assets.InstantiateAsync(data.prefabKey, parent);
        }
    }
}
