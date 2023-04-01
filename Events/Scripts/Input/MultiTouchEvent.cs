using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Utilities;

namespace Celeste.Events
{
    [Serializable]
    public struct MultiTouchEventArgs
    {
        public int touchCount;
        public ReadOnlyArray<UnityEngine.InputSystem.EnhancedTouch.Touch> touches;
    }

    [Serializable]
    public class MultiTouchUnityEvent : UnityEvent<MultiTouchEventArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(MultiTouchEvent), menuName = "Celeste/Events/Input/Multi Touch Event")]
    public class MultiTouchEvent : ParameterisedEvent<MultiTouchEventArgs>
    {
    }
}
