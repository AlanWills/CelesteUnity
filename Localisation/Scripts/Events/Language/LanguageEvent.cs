using Celeste.Localisation;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class LanguageUnityEvent : UnityEvent<Language> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(LanguageEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Localisation/Language Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class LanguageEvent : ParameterisedEvent<Language>
    {
    }
}
