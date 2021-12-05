using Celeste.Events;
using Celeste.UI;
using PolyAndCode.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Inventory.UI
{
    [AddComponentMenu("Celeste/Inventory/UI/Inventory Popup Controller")]
    public class InventoryPopupController : MonoBehaviour, IPopupController, IRecyclableScrollRectDataSource
    {
        #region Properties and Fields

        [Header("Data")]
        [SerializeField] private InventoryRecord inventory;

        [Header("Items")]
        [SerializeField] private RecyclableScrollRect scrollRect;

        [NonSerialized] private List<InventoryItemUIData> inventoryItemUIData = new List<InventoryItemUIData>();

        #endregion

        #region IPopupController

        public void OnShow(IPopupArgs args)
        {
            for (int i = 0, n = inventory.NumItems; i < n; ++i)
            {
                InventoryItem inventoryItem = inventory.GetItem(i);
                inventoryItemUIData.Add(new InventoryItemUIData(inventoryItem));
            }

            scrollRect.DataSource = this;
            scrollRect.ReloadData();
        }

        public void OnHide()
        {
            inventoryItemUIData.Clear();
        }

        public void OnConfirmPressed()
        {
        }

        public void OnClosePressed()
        {
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