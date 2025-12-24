using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class StringUnityEvent : UnityEvent<string> { }

    [Serializable]
    [CreateAssetMenu(fileName = "StringEvent", menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "String/String Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class StringEvent : ParameterisedEvent<string> { }
    
    [Serializable]
    public class GuaranteedStringEvent : GuaranteedParameterisedEvent<StringEvent, string> { }
}
