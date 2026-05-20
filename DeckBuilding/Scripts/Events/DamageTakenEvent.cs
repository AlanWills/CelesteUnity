using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public struct DamageTakenArgs
    {
        public CardInstance cardInstance;
        public int damageTaken;

        public DamageTakenArgs(CardInstance cardInstance, int damageTaken)
        {
            this.cardInstance = cardInstance;
            this.damageTaken = damageTaken;
        }
    }

    [Serializable]
    public class DamageTakenEvent : UnityEvent<DamageTakenArgs> { }
}