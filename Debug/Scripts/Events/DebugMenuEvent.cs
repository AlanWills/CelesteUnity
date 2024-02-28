using Celeste.Debug.Menus;
using Celeste.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Debug.Events
{
    [Serializable]
    public class DebugMenuUnityEvent : UnityEvent<DebugMenu> { }

    [CreateAssetMenu(fileName = nameof(DebugMenuEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Debug/Debug Menu Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class DebugMenuEvent : ParameterisedEvent<DebugMenu> { }
}