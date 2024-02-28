using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Inventory.Parameters
{
    [CreateAssetMenu(fileName = nameof(InventoryItemValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Inventory/Inventory Item Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class InventoryItemValue : ParameterValue<InventoryItem, InventoryItemValueChangedEvent>
    {
    }
}
