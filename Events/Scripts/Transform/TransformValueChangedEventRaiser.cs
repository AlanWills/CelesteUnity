using UnityEngine;
using Celeste.Events;

namespace Celeste.Events
{
	public class TransformValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<Transform>, TransformValueChangedEvent>
	{
	}
}
