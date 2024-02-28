using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class LongUnityEvent : UnityEvent<long> { }

    [CreateAssetMenu(fileName = nameof(LongEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Long Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class LongEvent : ParameterisedEvent<long>
    {
    }
}
