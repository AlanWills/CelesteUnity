#if USE_NEW_INPUT_SYSTEM
using UnityEngine.InputSystem;

namespace Celeste.Events
{
	public class KeyValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<Key>, KeyValueChangedEvent, KeyValueChangedUnityEvent>
	{
	}
}
#endif
