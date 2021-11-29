using System.Collections;
using UnityEngine;

namespace Celeste.Inventory.UI
{
    public class InventoryItemUIData
    {
        #region Properties and Fields

        public string Name
        {
            get { return InventoryItem.DisplayName; }
        }

        public Sprite Sprite
        {
            get { return InventoryItem.Sprite; }
        }

        private InventoryItem InventoryItem { get; }

        #endregion

        public InventoryItemUIData(InventoryItem inventoryItem)
        {
            InventoryItem = inventoryItem;
        }
    }
}