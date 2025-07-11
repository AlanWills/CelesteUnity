using Celeste.Narrative.Backgrounds;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class BackgroundUnityEvent : UnityEvent<Background> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(BackgroundEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Narrative/Background Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class BackgroundEvent : ParameterisedEvent<Background>
    {
    }
}
