using Celeste.DeckBuilding.Match;
using Celeste.Events;
using UnityEngine;

namespace Celeste.DeckBuilding.Events
{
    [AddComponentMenu("Celeste/Deck Building/Events/Save Deck Match Event Listener")]
    public class SaveDeckMatchEventListener : ParameterisedEventListener<SaveDeckMatchArgs, SaveDeckMatchEvent, SaveDeckMatchUnityEvent>
    {
    }
}
