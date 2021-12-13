using Celeste.Objects;
using UnityEngine;

namespace Celeste.Inventory
{
    [CreateAssetMenu(fileName = nameof(InventoryItemCatalogue), menuName = "Celeste/Inventory/Inventory Item Catalogue")]
    public class InventoryItemCatalogue : ArrayScriptableObject<InventoryItem>
    {
        public InventoryItem[] startingItems;
        
        public InventoryItem FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}