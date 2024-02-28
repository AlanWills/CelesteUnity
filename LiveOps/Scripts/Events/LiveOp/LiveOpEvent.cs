using Celeste.LiveOps;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class LiveOpUnityEvent : UnityEvent<LiveOp> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(LiveOpEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Live Ops/Live Op Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class LiveOpEvent : ParameterisedEvent<LiveOp>
    {
    }
}
