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
    public struct SaveDeckMatchArgs
    {
        public DeckMatchRuntime deckMatchRuntime;
        public FSMRuntime deckMatchFSMRuntime;

        public SaveDeckMatchArgs(
            DeckMatchRuntime deckMatchRuntime,
            FSMRuntime deckMatchFSMRuntime)
        {
            this.deckMatchRuntime = deckMatchRuntime;
            this.deckMatchFSMRuntime = deckMatchFSMRuntime;
        }
    }

    [Serializable]
    public class SaveDeckMatchUnityEvent : UnityEvent<SaveDeckMatchArgs> { }

    [CreateAssetMenu(fileName = nameof(SaveDeckMatchEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Deck Building/Save Deck Match Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class SaveDeckMatchEvent : ParameterisedEvent<SaveDeckMatchArgs> { }
}