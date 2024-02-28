using System;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;

namespace Celeste.Events
{
	[Serializable]
	public class CameraUnityEvent : UnityEvent<Camera> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(CameraEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Viewport/Camera Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class CameraEvent : ParameterisedEvent<Camera>
	{
	}
}
