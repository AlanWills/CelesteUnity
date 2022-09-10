using System;
using UnityEngine;
using Celeste.Events;
using Celeste.Inventory;

namespace Celeste.Events 
{
	[Serializable]
	public class InventoryItemValueChangedUnityEvent : ValueChangedUnityEvent<InventoryItem> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(InventoryItemValueChangedEvent), menuName = "Celeste/Events/InventoryItem/Inventory Item Value Changed Event")]
	public class InventoryItemValueChangedEvent : ParameterisedValueChangedEvent<InventoryItem>
	{
	}
}
