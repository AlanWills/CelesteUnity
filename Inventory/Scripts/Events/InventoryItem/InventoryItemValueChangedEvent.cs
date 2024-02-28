using System;
using UnityEngine;
using Celeste.Events;
using Celeste.Inventory;

namespace Celeste.Events 
{
	[Serializable]
	public class InventoryItemValueChangedUnityEvent : ValueChangedUnityEvent<InventoryItem> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(InventoryItemValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "InventoryItem/Inventory Item Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class InventoryItemValueChangedEvent : ParameterisedValueChangedEvent<InventoryItem>
	{
	}
}
