using UnityEngine;
using Celeste.Events;
using Celeste.Loading;

namespace Celeste.Events
{
	public class LoadJobValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<LoadJob>, LoadJobValueChangedEvent>
	{
	}
}
