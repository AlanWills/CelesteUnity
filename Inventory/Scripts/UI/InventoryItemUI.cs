﻿using PolyAndCode.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Inventory.UI
{
    [AddComponentMenu("Celeste/Inventory/UI/Inventory Item UI")]
    public class InventoryItemUI : MonoBehaviour, ICell
    {
        #region Properties and Fields

        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private Image itemImage;

        #endregion

        public void ConfigureCell(InventoryItemUIData inventoryItemUIData, int cellIndex)
        {
            itemName.text = inventoryItemUIData.Name;
            itemImage.sprite = inventoryItemUIData.Sprite;
        }
    }
}