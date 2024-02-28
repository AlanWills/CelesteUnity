using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class KeyCodeValueChangedUnityEvent : ValueChangedUnityEvent<KeyCode> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(KeyCodeValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Input/Key Code Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class KeyCodeValueChangedEvent : ParameterisedValueChangedEvent<KeyCode>
	{
	}
}
