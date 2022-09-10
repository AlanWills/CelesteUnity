using UnityEngine;
using Celeste.Events;

namespace Celeste.Events
{
	public class FloatValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<float>, FloatValueChangedEvent, FloatValueChangedUnityEvent>
	{
	}
}
