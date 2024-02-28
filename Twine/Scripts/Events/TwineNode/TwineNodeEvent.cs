using Celeste.Twine;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class TwineNodeUnityEvent : UnityEvent<TwineNode> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(TwineNodeEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Twine/Twine Node Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class TwineNodeEvent : ParameterisedEvent<TwineNode>
    {
    }
}
