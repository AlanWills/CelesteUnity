using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public struct DieArgs
    {
        public CardInstance cardInstance;

        public DieArgs(CardInstance cardInstance)
        {
            this.cardInstance = cardInstance;
        }
    }

    [Serializable]
    public class DieUnityEvent : UnityEvent<DieArgs> { }
}