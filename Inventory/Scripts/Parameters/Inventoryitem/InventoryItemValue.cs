using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Inventory.Parameters
{
    [CreateAssetMenu(fileName = nameof(InventoryItemValue), menuName = "Celeste/Parameters/Inventory/Inventory Item Value")]
    public class InventoryItemValue : ParameterValue<InventoryItem, InventoryItemValueChangedEvent>
    {
    }
}
