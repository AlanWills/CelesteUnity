using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Touch Event Listener")]
    public class TouchEventListener : ParameterisedEventListener<UnityEngine.InputSystem.EnhancedTouch.Touch, TouchEvent, TouchUnityEvent>
    {
    }
}
