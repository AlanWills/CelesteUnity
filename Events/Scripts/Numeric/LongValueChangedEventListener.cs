using UnityEngine;
using Celeste.Events;

namespace Celeste.Events
{
	public class LongValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<long>, LongValueChangedEvent, LongValueChangedUnityEvent>
	{
	}
}
