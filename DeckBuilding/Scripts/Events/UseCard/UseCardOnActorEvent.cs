using Celeste.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public struct UseCardOnActorArgs
    {
        public CardRuntime cardRuntime;
        public CardRuntime actor;
    }

    [Serializable]
    public class UseCardOnActorUnityEvent : UnityEvent<UseCardOnActorArgs> { }

    [CreateAssetMenu(fileName = nameof(UseCardOnActorEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Deck Building/Use Card On Actor Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class UseCardOnActorEvent : ParameterisedEvent<UseCardOnActorArgs> { }
}