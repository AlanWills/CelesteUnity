using UnityEngine;
using Celeste.Events;

namespace Celeste.Events
{
	public class StringValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<string>, StringValueChangedEvent, StringValueChangedUnityEvent>
	{
	}
}
