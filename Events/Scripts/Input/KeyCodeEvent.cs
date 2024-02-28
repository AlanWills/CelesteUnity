using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class KeyCodeUnityEvent : UnityEvent<KeyCode> { }

    [CreateAssetMenu(fileName = nameof(KeyCode), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Input/KeyCode Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class KeyCodeEvent : ParameterisedEvent<KeyCode>
    {
    }
}
