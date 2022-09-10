using UnityEngine;
using Celeste.Events;

namespace Celeste.Events
{
	public class LongValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<long>, LongValueChangedEvent>
	{
	}
}
