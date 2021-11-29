using Celeste.Events;
using Celeste.Inventory;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Inventory/Inventory Item Event Listener")]
    public class InventoryItemEventListener : ParameterisedEventListener<InventoryItem, InventoryItemEvent, InventoryItemUnityEvent>
    {
    }
}
