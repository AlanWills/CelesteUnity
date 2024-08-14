#if USE_ADDRESSABLES
using Celeste.DataStructures;
using Celeste.Inventory.AssetReferences;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Inventory.Settings
{
    [CreateAssetMenu(fileName = nameof(InventorySettingsUsingAddressables), menuName = CelesteMenuItemConstants.INVENTORY_MENU_ITEM + "Inventory Settings Using Addressables", order = CelesteMenuItemConstants.INVENTORY_MENU_ITEM_PRIORITY)]
    public class InventorySettingsUsingAddressables : InventorySettings
    {
        #region Properties and Fields

        public override ReadOnlyCollection<InventoryItem> StartingItems => inventoryItemCatalogue.Asset.startingItems.ToReadOnly();

        [SerializeField] private InventoryItemCatalogueAssetReference inventoryItemCatalogue;
        [SerializeField] private InventoryItemEventAssetReference inventoryItemAddedEvent;

        [NonSerialized] private bool loaded = false;

        #endregion

        public override bool ShouldLoadAssets()
        {
            return loaded;
        }

        public override IEnumerator LoadAssets()
        {
            yield return inventoryItemCatalogue.LoadAssetAsync();
            yield return inventoryItemAddedEvent.LoadAssetAsync();

            loaded = true;
        }

        public override InventoryItem MustFindInventoryItemByGuid(int guid)
        {
            return inventoryItemCatalogue.Asset.FindByGuid(guid);
        }

        public override void AddInventoryItemAddedCallback(UnityAction<InventoryItem> inventoryItem)
        {
            inventoryItemAddedEvent.Asset.AddListener(inventoryItem);
        }

        public override void RemoveInventoryItemAddedCallback(UnityAction<InventoryItem> inventoryItem)
        {
            inventoryItemAddedEvent.Asset.RemoveListener(inventoryItem);
        }
    }
}
#endif