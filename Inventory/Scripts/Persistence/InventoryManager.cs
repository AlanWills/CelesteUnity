using Celeste.Assets;
using Celeste.Inventory.Persistence;
using Celeste.Inventory.Settings;
using Celeste.Persistence;
using System.Collections;
using UnityEngine;

namespace Celeste.Inventory
{
    [AddComponentMenu("Celeste/Inventory/Inventory Manager")]
    public class InventoryManager : PersistentSceneManager<InventoryManager, InventoryDTO>, IHasAssets
    {
        #region Properties and Fields

        public static readonly string FILE_NAME = "Inventory.dat";
        protected override string FileName => FILE_NAME;

        [SerializeField] private InventorySettings inventorySettings;
        [SerializeField] private InventoryRecord inventoryRecord;

        #endregion

        #region IHasAssets
        
        public bool ShouldLoadAssets()
        {
            return inventorySettings.ShouldLoadAssets();
        }

        public IEnumerator LoadAssets()
        {
            yield return inventorySettings.LoadAssets();

            inventorySettings.AddInventoryItemAddedCallback(OnAddItemToInventory);

            Load();
        }

        #endregion

        #region Unity Methods

        protected override void OnDestroy()
        {
            inventoryRecord.Shutdown();

            base.OnDestroy();
        }

        #endregion

        #region Save/Load Methods

        protected override void Deserialize(InventoryDTO dto)
        {
            inventoryRecord.Initialize(dto.maxSize, dto.itemGuids, inventorySettings);
        }

        protected override InventoryDTO Serialize()
        {
            return new InventoryDTO(inventoryRecord);
        }

        protected override void SetDefaultValues()
        {
            inventoryRecord.CreateStartingInventory(inventorySettings.StartingItems);
        }

        #endregion

        #region Callbacks

        private void OnAddItemToInventory(InventoryItem item)
        {
            if (!inventoryRecord.IsFull)
            {
                inventoryRecord.AddItem(item);
            }
        }

        #endregion
    }
}