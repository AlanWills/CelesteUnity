using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public struct DieArgs
    {
        public CardRuntime cardRuntime;

        public DieArgs(CardRuntime cardRuntime)
        {
            this.cardRuntime = cardRuntime;
        }
    }

    [Serializable]
    public class DieUnityEvent : UnityEvent<DieArgs> { }
}