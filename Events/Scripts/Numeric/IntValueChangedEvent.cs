using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class IntValueChangedUnityEvent : ValueChangedUnityEvent<int> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(IntValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Numeric/Int Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class IntValueChangedEvent : ParameterisedValueChangedEvent<int>
	{
	}
}
