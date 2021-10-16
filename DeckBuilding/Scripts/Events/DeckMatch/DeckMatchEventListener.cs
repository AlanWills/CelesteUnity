using Celeste.DeckBuilding.Match;
using Celeste.Events;
using UnityEngine;

namespace Celeste.DeckBuilding.Events
{
    [AddComponentMenu("Celeste/Deck Building/Events/Deck Match Event Listener")]
    public class DeckMatchEventListener : ParameterisedEventListener<DeckMatch, DeckMatchEvent, DeckMatchUnityEvent>
    {
    }
}
