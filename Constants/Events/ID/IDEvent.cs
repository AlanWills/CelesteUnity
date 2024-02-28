using Celeste.Constants;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class IDUnityEvent : UnityEvent<ID> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(ID), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Constants/ID Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class IDEvent : ParameterisedEvent<ID>
    {
    }
}
