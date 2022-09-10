using UnityEngine;
using Celeste.Events;

namespace Celeste.Events
{
	public class GameObjectValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<GameObject>, GameObjectValueChangedEvent>
	{
	}
}
