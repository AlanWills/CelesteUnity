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

    [CreateAssetMenu(fileName = nameof(UseCardOnActorEvent), menuName = "Celeste/Events/Deck Building/Use Card On Actor Event")]
    public class UseCardOnActorEvent : ParameterisedEvent<UseCardOnActorArgs> { }
}