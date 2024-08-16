#if USE_NEW_INPUT_SYSTEM
using Key = UnityEngine.InputSystem.Key;
#else
using Key = UnityEngine.KeyCode;
#endif

namespace Celeste.Events
{
	public class KeyValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<Key>, KeyValueChangedEvent>
	{
	}
}