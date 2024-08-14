using System;
using UnityEngine;
#if USE_NEW_INPUT_SYSTEM
using Key = UnityEngine.InputSystem.Key;
#else
using Key = UnityEngine.KeyCode;
#endif

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