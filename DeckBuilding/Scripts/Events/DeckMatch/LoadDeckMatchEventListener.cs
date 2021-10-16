using Celeste.DeckBuilding.Match;
using Celeste.Events;
using UnityEngine;

namespace Celeste.DeckBuilding.Events
{
    [AddComponentMenu("Celeste/Deck Building/Events/Load Deck Match Event Listener")]
    public class LoadDeckMatchEventListener : ParameterisedEventListener<LoadDeckMatchArgs, LoadDeckMatchEvent, LoadDeckMatchUnityEvent>
    {
    }
}
