using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class CameraValueChangedUnityEvent : ValueChangedUnityEvent<Camera> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(CameraValueChangedEvent), menuName = "Celeste/Events/Viewport/Camera Value Changed Event")]
	public class CameraValueChangedEvent : ParameterisedValueChangedEvent<Camera>
	{
	}
}
