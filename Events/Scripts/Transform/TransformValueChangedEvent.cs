using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class TransformValueChangedUnityEvent : ValueChangedUnityEvent<Transform> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(TransformValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Transform/Transform Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class TransformValueChangedEvent : ParameterisedValueChangedEvent<Transform>
	{
	}
}
