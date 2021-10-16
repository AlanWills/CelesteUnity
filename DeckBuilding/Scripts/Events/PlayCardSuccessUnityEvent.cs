using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public struct PlayCardSuccessArgs
    {
        public CardRuntime cardRuntime;
    }

    [Serializable]
    public class PlayCardSuccessUnityEvent : UnityEvent<PlayCardSuccessArgs> { }
}