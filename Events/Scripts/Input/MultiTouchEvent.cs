using System;
using System.Collections.Generic;
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
    public struct MultiTouchEventArgs
    {
        public int touchCount;
        public IReadOnlyList<Touch> touches;
    }

    [Serializable]
    public class MultiTouchUnityEvent : UnityEvent<MultiTouchEventArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(MultiTouchEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Input/Multi Touch Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class MultiTouchEvent : ParameterisedEvent<MultiTouchEventArgs>
    {
    }
}