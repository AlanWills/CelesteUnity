using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Bool/Bool Value Changed Event Listener")]
    public class BoolValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<bool>, BoolValueChangedEvent, BoolValueChangedUnityEvent>
    {
    }
}
