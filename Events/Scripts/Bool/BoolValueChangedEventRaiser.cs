using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Bool/Bool Value Changed Event Raiser")]
    public class BoolValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<bool>, BoolValueChangedEvent>
    {
    }
}
