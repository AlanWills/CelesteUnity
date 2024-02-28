using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class StringValueChangedUnityEvent : ValueChangedUnityEvent<string> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(StringValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "String/String Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class StringValueChangedEvent : ParameterisedValueChangedEvent<string>
	{
	}
}
