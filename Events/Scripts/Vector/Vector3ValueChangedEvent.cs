using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class Vector3ValueChangedUnityEvent : ValueChangedUnityEvent<Vector3> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(Vector3ValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Vector3/Vector3 Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class Vector3ValueChangedEvent : ParameterisedValueChangedEvent<Vector3>
	{
	}
}
