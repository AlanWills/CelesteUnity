using UnityEngine;
using Celeste.Events;
using Celeste.Inventory;

namespace Celeste.Events
{
	public class InventoryItemValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<InventoryItem>, InventoryItemValueChangedEvent>
	{
	}
}
