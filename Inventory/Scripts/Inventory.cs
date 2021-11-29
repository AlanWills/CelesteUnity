using Celeste.DataStructures;
using Celeste.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Inventory
{
    [CreateAssetMenu(fileName = nameof(Inventory), menuName = "Celeste/Inventory/Inventory")]
    public class Inventory : ScriptableObject
    {
        #region Properties and Fields

        public int NumItems
        {
            get { return items.Count; }
        }

        [SerializeField] private List<InventoryItem> startingItems = new List<InventoryItem>();

        [Header("Events")]
        [SerializeField] private InventoryItemEvent onItemAdded;
        [SerializeField] private InventoryItemEvent onItemRemoved;
        [SerializeField] private Events.Event save;

        [NonSerialized] private List<InventoryItem> items = new List<InventoryItem>();

        #endregion

        #region Item Management

        public void CreateStartingInventory()
        {
            items.Clear();
            items.AddRange(startingItems);
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
            items.Add(inventoryItem);
            onItemAdded.Invoke(inventoryItem);
            save.Invoke();
        }

        public void RemoveItem(int index)
        {
#if INDEX_CHECKS
            if (0 <= index && index < NumItems)
#endif
            {
                RemoveItem(items[index]);
            }
        }

        public void RemoveItem(InventoryItem inventoryItem)
        {
            items.Remove(inventoryItem);
            onItemRemoved.Invoke(inventoryItem);
            save.Invoke();
        }

        #endregion
    }
}