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

    [CreateAssetMenu(fileName = nameof(AttackActorWithActorEvent), menuName = "Celeste/Events/Deck Building/Attack Actor With Actor Event")]
    public class AttackActorWithActorEvent : ParameterisedEvent<AttackActorWithActorArgs> { }
}