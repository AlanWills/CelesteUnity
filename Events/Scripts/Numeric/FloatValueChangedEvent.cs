using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class FloatValueChangedUnityEvent : ValueChangedUnityEvent<float> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(FloatValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Numeric/Float Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class FloatValueChangedEvent : ParameterisedValueChangedEvent<float>
	{
	}
}
