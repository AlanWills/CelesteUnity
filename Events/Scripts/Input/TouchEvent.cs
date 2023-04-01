using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class TouchUnityEvent : UnityEvent<UnityEngine.InputSystem.EnhancedTouch.Touch> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(TouchEvent), menuName = "Celeste/Events/Input/Touch Event")]
    public class TouchEvent : ParameterisedEvent<UnityEngine.InputSystem.EnhancedTouch.Touch>
    {
    }
}
