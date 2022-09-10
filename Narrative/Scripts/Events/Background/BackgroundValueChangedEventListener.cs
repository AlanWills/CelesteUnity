using UnityEngine;
using Celeste.Events;
using Celeste.Narrative.Backgrounds;

namespace Celeste.Events
{
	public class BackgroundValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<Background>, BackgroundValueChangedEvent, BackgroundValueChangedUnityEvent>
	{
	}
}
