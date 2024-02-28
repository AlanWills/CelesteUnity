using Celeste.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public struct AttackActorWithActorArgs
    {
        public CardRuntime attacker;
        public CardRuntime target;
    }

    [Serializable]
    public class AttackActorWithActorUnityEvent : UnityEvent<AttackActorWithActorArgs> { }

    [CreateAssetMenu(fileName = nameof(AttackActorWithActorEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Deck Building/Attack Actor With Actor Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class AttackActorWithActorEvent : ParameterisedEvent<AttackActorWithActorArgs> { }
}