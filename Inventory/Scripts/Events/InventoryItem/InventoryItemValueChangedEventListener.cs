using UnityEngine;
using Celeste.Events;
using Celeste.Inventory;

namespace Celeste.Events
{
	public class InventoryItemValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<InventoryItem>, InventoryItemValueChangedEvent, InventoryItemValueChangedUnityEvent>
	{
	}
}
