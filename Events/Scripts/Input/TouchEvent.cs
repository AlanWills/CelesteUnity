using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class TouchUnityEvent : UnityEvent<UnityEngine.InputSystem.EnhancedTouch.Touch> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(TouchEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Input/Touch Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class TouchEvent : ParameterisedEvent<UnityEngine.InputSystem.EnhancedTouch.Touch>
    {
    }
}
