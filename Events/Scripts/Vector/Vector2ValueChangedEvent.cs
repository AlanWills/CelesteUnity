using System;
using UnityEngine;

namespace Celeste.Events 
{
	[Serializable]
	public class Vector2ValueChangedUnityEvent : ValueChangedUnityEvent<Vector2> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(Vector2ValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Vector2/Vector2 Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class Vector2ValueChangedEvent : ParameterisedValueChangedEvent<Vector2>
	{
	}
}
