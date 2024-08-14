#if USE_NEW_INPUT_SYSTEM
using UnityEngine.InputSystem;

namespace Celeste.Events
{
	public class KeyValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<Key>, KeyValueChangedEvent>
	{
	}
}
#endif