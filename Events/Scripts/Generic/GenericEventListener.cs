using UnityEngine;
using Celeste.Events;

namespace Celeste.Events
{
	public class GenericEventListener : ParameterisedEventListener<IEventArgs, GenericEvent, GenericUnityEvent> { }
}
