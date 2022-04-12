using Celeste.Assets;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Celeste.Inventory.Settings
{
    public abstract class InventorySettings : ScriptableObject, IHasAssets
    {
        #region Properties and Fields

        public abstract ReadOnlyCollection<InventoryItem> StartingItems { get; }

        #endregion

        public abstract bool ShouldLoadAssets();
        public abstract IEnumerator LoadAssets();

        public abstract InventoryItem MustFindInventoryItemByGuid(int guid);

        public abstract void AddInventoryItemAddedCallback(Action<InventoryItem> inventoryItem);
        public abstract void RemoveInventoryItemAddedCallback(Action<InventoryItem> inventoryItem);
    }
}
