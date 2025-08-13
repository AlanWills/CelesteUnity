using Celeste.Objects;
using UnityEngine;

namespace Celeste.Inventory
{
    [CreateAssetMenu(fileName = nameof(InventoryItemCatalogue), menuName = CelesteMenuItemConstants.INVENTORY_MENU_ITEM + "Inventory Item Catalogue", order = CelesteMenuItemConstants.INVENTORY_MENU_ITEM_PRIORITY)]
    public class InventoryItemCatalogue : ListScriptableObject<InventoryItem>
    {
        public InventoryItem[] startingItems;
        
        public InventoryItem FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}