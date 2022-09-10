using UnityEngine;
using Celeste.Events;

namespace Celeste.Events
{
	public class TransformValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<Transform>, TransformValueChangedEvent, TransformValueChangedUnityEvent>
	{
	}
}
