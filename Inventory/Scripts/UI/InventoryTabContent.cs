using PolyAndCode.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Inventory.UI
{
    [AddComponentMenu("Celeste/Inventory/UI/Inventory Tab Content")]
    public class InventoryTabContent : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        #region Properties and Fields

        [Header("Data")]
        [SerializeField] private InventoryRecord inventory;

        [Header("Items")]
        [SerializeField] private RecyclableScrollRect scrollRect;

        [NonSerialized] private List<InventoryItemUIData> inventoryItemUIData = new List<InventoryItemUIData>();

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            for (int i = 0, n = inventory.NumItems; i < n; ++i)
            {
                InventoryItem inventoryItem = inventory.GetItem(i);
                inventoryItemUIData.Add(new InventoryItemUIData(inventoryItem));
            }

            scrollRect.DataSource = this;
            scrollRect.ReloadData();
        }

        private void OnDisable()
        {
            inventoryItemUIData.Clear();
        }

        #endregion

        #region Data Source Methods

        public int GetItemCount()
        {
            return inventoryItemUIData.Count;
        }

        public void SetCell(ICell cell, int index)
        {
            InventoryItemUI inventoryItemUI = cell as InventoryItemUI;
            inventoryItemUI.ConfigureCell(inventoryItemUIData[index], index);
        }

        #endregion
    }
}