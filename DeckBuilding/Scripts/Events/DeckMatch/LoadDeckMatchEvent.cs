using Celeste.DeckBuilding.Match;
using Celeste.Events;
using Celeste.FSM;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public struct LoadDeckMatchArgs
    {
        public DeckMatchRuntime deckMatchRuntime;
        public FSMRuntime deckMatchFSMRuntime;

        public LoadDeckMatchArgs(
            DeckMatchRuntime deckMatchRuntime, 
            FSMRuntime deckMatchFSMRuntime)
        {
            this.deckMatchRuntime = deckMatchRuntime;
            this.deckMatchFSMRuntime = deckMatchFSMRuntime;
        }
    }

    [Serializable]
    public class LoadDeckMatchUnityEvent : UnityEvent<LoadDeckMatchArgs> { }

    [CreateAssetMenu(fileName = nameof(LoadDeckMatchEvent), menuName = "Celeste/Events/Deck Building/Load Deck Match Event")]
    public class LoadDeckMatchEvent : ParameterisedEvent<LoadDeckMatchArgs> { }
}