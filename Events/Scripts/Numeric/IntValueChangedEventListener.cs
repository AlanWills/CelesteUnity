using UnityEngine;
using Celeste.Events;

namespace Celeste.Events
{
	public class IntValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<int>, IntValueChangedEvent, IntValueChangedUnityEvent>
	{
	}
}
