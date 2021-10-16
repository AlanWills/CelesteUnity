using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public struct PlayCardFailureArgs
    {
        public CardRuntime cardRuntime;
    }

    [Serializable]
    public class PlayCardFailureUnityEvent : UnityEvent<PlayCardFailureArgs> { }
}