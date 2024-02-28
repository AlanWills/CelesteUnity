using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class Vector3IntUnityEvent : UnityEvent<Vector3Int> { }

    [Serializable]
    [CreateAssetMenu(fileName = "Vector3IntEvent", menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Vector3Int/Vector3Int Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class Vector3IntEvent : ParameterisedEvent<Vector3Int> { }
}
