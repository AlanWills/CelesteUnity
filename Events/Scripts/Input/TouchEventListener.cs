using UnityEngine;
#if USE_NEW_INPUT_SYSTEM
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
#else
using Touch = UnityEngine.Touch;
#endif

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Touch Event Listener")]
    public class TouchEventListener : ParameterisedEventListener<Touch, TouchEvent, TouchUnityEvent>
    {
    }
}