using Celeste.Inventory;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Inventory/Inventory Item Event Raiser")]
    public class BackgroundEventRaiser : ParameterisedEventRaiser<InventoryItem, InventoryItemEvent>
    {
    }
}
