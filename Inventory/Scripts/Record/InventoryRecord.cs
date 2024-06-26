using Celeste.DataStructures;
using Celeste.Events;
using Celeste.Inventory.Settings;
using Celeste.Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Inventory
{
    [CreateAssetMenu(fileName = nameof(InventoryRecord), menuName = CelesteMenuItemConstants.INVENTORY_MENU_ITEM + "Inventory Record", order = CelesteMenuItemConstants.INVENTORY_MENU_ITEM_PRIORITY)]
    public class InventoryRecord : ScriptableObject
    {
        #region Properties and Fields

        public int NumItems => items.Count;
        public int MaxSize { get; private set; }
        public bool IsFull => NumItems >= MaxSize;

        [SerializeField] private int startingMaxSize = 5;

        [Header("Events")]
        [SerializeField] private InventoryItemEvent onItemAdded;
        [SerializeField] private InventoryItemEvent onItemRemoved;
        [SerializeField] private Events.Event save;

        [NonSerialized] private List<InventoryItem> items = new List<InventoryItem>();

        #endregion

        #region Item Management

        public void Initialize(int maxSize, List<int> inventoryItemGuids, InventorySettings inventorySettings)
        {
            Shutdown();

            MaxSize = maxSize;

            foreach (int itemGuid in inventoryItemGuids)
            {
                InventoryItem item = inventorySettings.MustFindInventoryItemByGuid(itemGuid);

                if (item != null)
                {
                    AddItemNoNotify(item);
                }
            }
        }

        public void Shutdown()
        {
            for (int i = NumItems - 1; i >= 0; --i)
            {
                RemoveItemNoNotify(i);
            }

            items.Clear();
        }

        public void Clear()
        {
            for (int i = NumItems - 1; i>= 0; --i)
            {
                RemoveItem(i);
            }
        }

        public void CreateStartingInventory(IList<InventoryItem> startingItems)
        {
            items.AddRange(startingItems);

            MaxSize = startingMaxSize;
        }

        public InventoryItem GetItem(int index)
        {
            return items.Get(index);
        }

        public InventoryItem FindItem(Predicate<InventoryItem> predicate)
        {
            return items.Find(predicate);
        }

        public void AddItem(InventoryItem inventoryItem)
        {
            if (AddItemNoNotify(inventoryItem))
            {
                save.Invoke();
            }
        }

        private bool AddItemNoNotify(InventoryItem inventoryItem)
        {
            UnityEngine.Debug.Assert(!IsFull, $"Failed to add item to full inventory.");
            if (!IsFull)
            {
                items.Add(inventoryItem);
                onItemAdded.Invoke(inventoryItem);
                
                return true;
            }

            return false;
        }

        public void RemoveItem(int index)
        {
            if (RemoveItemNoNotify(index))
            {
                save.Invoke();
            }
        }

        public bool RemoveItemNoNotify(int index)
        {
#if INDEX_CHECKS
            if (0 <= index && index < NumItems)
#endif
            {
                InventoryItem inventoryItem = items[index];
                items.RemoveAt(index);
                onItemRemoved.Invoke(inventoryItem);

                return true;
            }

            return false;
        }

        #endregion
    }
}