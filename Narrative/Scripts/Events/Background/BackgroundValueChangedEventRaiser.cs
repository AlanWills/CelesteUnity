using UnityEngine;
using Celeste.Events;
using Celeste.Narrative.Backgrounds;

namespace Celeste.Events
{
	public class BackgroundValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<Background>, BackgroundValueChangedEvent>
	{
	}
}
