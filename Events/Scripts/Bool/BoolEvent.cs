using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class BoolUnityEvent : UnityEvent<bool> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(BoolEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Bool/Bool Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class BoolEvent : ParameterisedEvent<bool> { }

    [Serializable]
    public class GuaranteedBoolEvent : GuaranteedParameterisedEvent<BoolEvent, bool> { }
}
