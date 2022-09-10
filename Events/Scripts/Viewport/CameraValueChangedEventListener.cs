using UnityEngine;
using Celeste.Events;

namespace Celeste.Events
{
	public class CameraValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<Camera>, CameraValueChangedEvent, CameraValueChangedUnityEvent>
	{
	}
}
