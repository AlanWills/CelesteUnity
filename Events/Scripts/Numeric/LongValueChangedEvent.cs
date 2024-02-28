using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class LongValueChangedUnityEvent : ValueChangedUnityEvent<long> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(LongValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Numeric/Long Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class LongValueChangedEvent : ParameterisedValueChangedEvent<long>
	{
	}
}
