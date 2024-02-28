using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class CameraValueChangedUnityEvent : ValueChangedUnityEvent<Camera> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(CameraValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Viewport/Camera Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class CameraValueChangedEvent : ParameterisedValueChangedEvent<Camera>
	{
	}
}
