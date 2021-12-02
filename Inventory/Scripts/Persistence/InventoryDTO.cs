using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Inventory.Persistence
{
    [Serializable]
    public class InventoryDTO
    {
        public List<int> itemGuids;

        public InventoryDTO() 
        {
            itemGuids = new List<int>();
        }

        public InventoryDTO(InventoryRecord inventory)
        {
            itemGuids = new List<int>(inventory.NumItems);

            for (int i = 0; i < itemGuids.Count; ++i)
            {
                itemGuids.Add(inventory.GetItem(i).Guid);
            }
        }
    }
}