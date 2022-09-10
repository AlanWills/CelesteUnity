using System;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;

namespace Celeste.Events
{
	[Serializable]
	public class CameraUnityEvent : UnityEvent<Camera> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(CameraEvent), menuName = "Celeste/Events/Viewport/Camera Event")]
	public class CameraEvent : ParameterisedEvent<Camera>
	{
	}
}
