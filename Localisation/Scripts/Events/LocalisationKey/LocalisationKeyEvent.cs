using Celeste.Localisation;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class LocalisationKeyUnityEvent : UnityEvent<LocalisationKey> { }

    [CreateAssetMenu(fileName = nameof(LocalisationKeyEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Localisation/Localisation Key Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class LocalisationKeyEvent : ParameterisedEvent<LocalisationKey>
    {
    }
}
