using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class Vector3UnityEvent : UnityEvent<Vector3> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(Vector3Event), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Vector3/Vector3 Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class Vector3Event : ParameterisedEvent<Vector3> { }
}
