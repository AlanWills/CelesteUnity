using UnityEngine;
using Celeste.Events;

namespace Celeste.Events
{
	public class GameObjectValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<GameObject>, GameObjectValueChangedEvent, GameObjectValueChangedUnityEvent>
	{
	}
}
