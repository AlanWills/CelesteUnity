using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Inventory.Parameters
{
    [CreateAssetMenu(fileName = nameof(InventoryItemReference), menuName = "Celeste/Parameters/Inventory/Inventory Item Reference")]
    public class InventoryItemReference : ParameterReference<InventoryItem, InventoryItemValue, InventoryItemReference>
    {
    }
}
