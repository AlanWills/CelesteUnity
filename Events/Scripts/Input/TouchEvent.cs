using System;
using UnityEngine;
using UnityEngine.Events;
#if USE_NEW_INPUT_SYSTEM
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
#else
using Touch = UnityEngine.Touch;
#endif

namespace Celeste.Events
{
    [Serializable]
    public class TouchUnityEvent : UnityEvent<Touch> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(TouchEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Input/Touch Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class TouchEvent : ParameterisedEvent<Touch>
    {
    }
}