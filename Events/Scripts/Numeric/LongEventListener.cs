using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Long Event Listener")]
    public class LongEventListener : ParameterisedEventListener<long, LongEvent, LongUnityEvent>
    {
    }
}
