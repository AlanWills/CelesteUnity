using Celeste.Inventory.Persistence;
using Celeste.Persistence;
using System;
using UnityEngine;

namespace Celeste.Inventory
{
    [AddComponentMenu("Celeste/Inventory/Inventory Manager")]
    public class InventoryManager : PersistentSceneManager<InventoryManager, InventoryDTO>
    {
        #region Properties and Fields

        public static readonly string FILE_NAME = "Inventory.dat";
        protected override string FileName
        {
            get { return FILE_NAME; }
        }

        [SerializeField] private InventoryItemCatalogue inventoryItemCatalogue;
        [SerializeField] private InventoryRecord inventory;

        #endregion

        #region Save/Load Methods

        protected override void Deserialize(InventoryDTO dto)
        {
            foreach (int itemGuid in dto.itemGuids)
            {
                InventoryItem item = inventoryItemCatalogue.FindByGuid(itemGuid);
                Debug.Assert(item != null, $"Could not find inventory item with guid {itemGuid} in catalogue.");

                if (item != null)
                {
                    inventory.AddItem(item);
                }
            }
        }

        protected override InventoryDTO Serialize()
        {
            return new InventoryDTO(inventory);
        }

        protected override void SetDefaultValues()
        {
            inventory.CreateStartingInventory();
        }

        #endregion
    }
}