using Celeste.DeckBuilding.Match;
using Celeste.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public class DeckMatchUnityEvent : UnityEvent<DeckMatch> { }

    [CreateAssetMenu(fileName = nameof(DeckMatchEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Deck Building/Deck Match Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class DeckMatchEvent : ParameterisedEvent<DeckMatch> { }
}