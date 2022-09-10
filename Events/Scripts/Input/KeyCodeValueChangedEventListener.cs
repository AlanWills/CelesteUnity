using UnityEngine;
using Celeste.Events;

namespace Celeste.Events
{
	public class KeyCodeValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<KeyCode>, KeyCodeValueChangedEvent, KeyCodeValueChangedUnityEvent>
	{
	}
}
