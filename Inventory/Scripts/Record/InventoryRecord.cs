using Celeste.DataStructures;
using Celeste.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Inventory
{
    [CreateAssetMenu(fileName = nameof(InventoryRecord), menuName = "Celeste/Inventory/Inventory Record")]
    public class InventoryRecord : ScriptableObject
    {
        #region Properties and Fields

        public int NumItems
        {
            get { return items.Count; }
        }

        public int MaxSize { get; set; }

        public bool IsFull
        {
            get { return NumItems >= MaxSize; }
        }

        [SerializeField] private int startingMaxSize = 5;

        [Header("Events")]
        [SerializeField] private InventoryItemEvent onItemAdded;
        [SerializeField] private InventoryItemEvent onItemRemoved;
        [SerializeField] private Events.Event save;

        [NonSerialized] private List<InventoryItem> items = new List<InventoryItem>();

        #endregion

        #region Item Management

        public void Clear()
        {
            items.Clear();
        }

        public void CreateStartingInventory(InventoryItem[] startingItems)
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
            UnityEngine.Debug.Assert(!IsFull, $"Failed to add item to full inventory.");
            if (!IsFull)
            {
                items.Add(inventoryItem);
                onItemAdded.Invoke(inventoryItem);
                save.Invoke();
            }
        }

        public void RemoveItem(int index)
        {
#if INDEX_CHECKS
            if (0 <= index && index < NumItems)
#endif
            {
                InventoryItem inventoryItem = items[index];
                items.RemoveAt(index);
                onItemRemoved.Invoke(inventoryItem);
                save.Invoke();
            }
        }

        #endregion
    }
}