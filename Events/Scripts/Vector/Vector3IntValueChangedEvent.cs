using System;
using UnityEngine;

namespace Celeste.Events 
{
	[Serializable]
	public class Vector3IntValueChangedUnityEvent : ValueChangedUnityEvent<Vector3Int> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(Vector3IntValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Vector3Int/Vector3 Int Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class Vector3IntValueChangedEvent : ParameterisedValueChangedEvent<Vector3Int>
	{
	}
}
