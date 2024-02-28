using System;
using UnityEngine;
using Celeste.Events;
using UnityEngine.InputSystem;

namespace Celeste.Events 
{
	[Serializable]
	public class KeyValueChangedUnityEvent : ValueChangedUnityEvent<Key> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(KeyValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Input/Key Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class KeyValueChangedEvent : ParameterisedValueChangedEvent<Key>
	{
	}
}
