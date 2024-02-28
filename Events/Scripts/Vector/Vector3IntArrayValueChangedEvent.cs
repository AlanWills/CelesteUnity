using System;
using UnityEngine;
using Celeste.Events;
using System.Collections.Generic;

namespace Celeste.Events 
{
	[Serializable]
	public class Vector3IntArrayValueChangedUnityEvent : ValueChangedUnityEvent<List<Vector3Int>> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(Vector3IntArrayValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Vector3Int Array/Vector3Int Array Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class Vector3IntArrayValueChangedEvent : ParameterisedValueChangedEvent<List<Vector3Int>>
	{
	}
}
