using Celeste.Events;
using Celeste.FSM.Nodes.Events;
using Celeste.Inventory.Parameters;

namespace Celeste.Inventory.Nodes.Events
{
    [CreateNodeMenu("Celeste/Events/Raisers/InventoryItemEvent Raiser")]
    public class InventoryItemEventRaiserNode : ParameterisedEventRaiserNode<InventoryItem, InventoryItemValue, InventoryItemReference, InventoryItemEvent>
    {
    }
}