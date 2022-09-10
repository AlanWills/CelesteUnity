using UnityEngine;
using Celeste.Events;

namespace Celeste.Events
{
	public class CameraValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<Camera>, CameraValueChangedEvent>
	{
	}
}
