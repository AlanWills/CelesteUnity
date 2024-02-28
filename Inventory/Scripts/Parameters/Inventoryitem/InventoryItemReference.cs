using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Inventory.Parameters
{
    [CreateAssetMenu(fileName = nameof(InventoryItemReference), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Inventory/Inventory Item Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class InventoryItemReference : ParameterReference<InventoryItem, InventoryItemValue, InventoryItemReference>
    {
    }
}
