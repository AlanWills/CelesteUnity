using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class Vector2UnityEvent : UnityEvent<Vector2> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(Vector2Event), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Vector2/Vector2 Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class Vector2Event : ParameterisedEvent<Vector2> { }
}
