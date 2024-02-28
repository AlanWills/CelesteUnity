using Celeste.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public class CardRuntimeUnityEvent : UnityEvent<CardRuntime> { }

    [CreateAssetMenu(fileName = nameof(CardRuntimeEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Deck Building/Card Runtime Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class CardRuntimeEvent : ParameterisedEvent<CardRuntime> { }
}