using UnityEngine;
using Celeste.Events;
using Celeste.Loading;

namespace Celeste.Events
{
	public class LoadJobValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<LoadJob>, LoadJobValueChangedEvent, LoadJobValueChangedUnityEvent>
	{
	}
}
