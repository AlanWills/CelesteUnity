using Celeste.Events;
using Celeste.UI;
using PolyAndCode.UI;
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
        [SerializeField] private Inventory inventory;

        [Header("Items")]
        [SerializeField] private RecyclableScrollRect scrollRect;

        private List<InventoryItemUIData> inventoryItemUIData = new List<InventoryItemUIData>();

        #endregion

        #region IPopupController

        public void OnShow(ShowPopupArgs args)
        {
            for (int i = 0, n = inventory.NumItems; i < n; ++i)
            {
                InventoryItem inventoryItem = inventory.GetItem(i);
                inventoryItemUIData.Add(new InventoryItemUIData(inventoryItem));
            }

            gameObject.SetActive(true);
            scrollRect.DataSource = this;
            scrollRect.Initialize(this);
        }

        public void OnHide()
        {
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