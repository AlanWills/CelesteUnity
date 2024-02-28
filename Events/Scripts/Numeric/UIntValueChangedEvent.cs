using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class UIntValueChangedUnityEvent : ValueChangedUnityEvent<uint> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(UIntValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Numeric/UInt Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class UIntValueChangedEvent : ParameterisedValueChangedEvent<uint>
	{
	}
}
