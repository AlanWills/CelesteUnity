using Celeste.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public struct BeginAttackActorArgs
    {
        public CardRuntime attacker;
        public Vector3 position;
    }

    [Serializable]
    public class BeginAttackActorUnityEvent : UnityEvent<BeginAttackActorArgs> { }

    [CreateAssetMenu(fileName = nameof(BeginAttackActorEvent), menuName = "Celeste/Events/Deck Building/Begin Attack Actor")]
    public class BeginAttackActorEvent : ParameterisedEvent<BeginAttackActorArgs> { }
}