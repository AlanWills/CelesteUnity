using System;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;
using System.Collections.Generic;

namespace Celeste.Events
{
	[Serializable]
	public class Vector3IntArrayUnityEvent : UnityEvent<List<Vector3Int>> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(Vector3IntArrayEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Vector3Int Array/Vector3 Int Array Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class Vector3IntArrayEvent : ParameterisedEvent<List<Vector3Int>>
	{
	}
}
