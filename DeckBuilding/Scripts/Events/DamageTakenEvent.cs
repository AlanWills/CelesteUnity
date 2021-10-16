using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public struct DamageTakenArgs
    {
        public CardRuntime cardRuntime;
        public int damageTaken;

        public DamageTakenArgs(CardRuntime cardRuntime, int damageTaken)
        {
            this.cardRuntime = cardRuntime;
            this.damageTaken = damageTaken;
        }
    }

    [Serializable]
    public class DamageTakenEvent : UnityEvent<DamageTakenArgs> { }
}