using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class FloatUnityEvent : UnityEvent<float> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(FloatEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Numeric/Float Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class FloatEvent : ParameterisedEvent<float> { }

    [Serializable]
    public class GuaranteedFloatEvent : GuaranteedParameterisedEvent<FloatEvent, float> { }
}
