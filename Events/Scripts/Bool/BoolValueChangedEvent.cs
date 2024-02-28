using System;
using UnityEngine;

namespace Celeste.Events
{
    [Serializable]
    public class BoolValueChangedUnityEvent : ValueChangedUnityEvent<bool> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(BoolValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Bool/Bool Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class BoolValueChangedEvent : ParameterisedValueChangedEvent<bool> { }

    [Serializable]
    public class GuaranteedBoolValueChangedEvent : GuaranteedParameterisedValueChangedEvent<BoolValueChangedEvent, bool> { }
}
