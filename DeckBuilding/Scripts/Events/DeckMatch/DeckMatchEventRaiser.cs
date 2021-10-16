using Celeste.DeckBuilding.Match;
using Celeste.Events;
using UnityEngine;

namespace Celeste.DeckBuilding.Events
{
    [AddComponentMenu("Celeste/Deck Building/Events/Deck Match Event Raiser")]
    public class DeckMatchEventRaiser : ParameterisedEventRaiser<DeckMatch, DeckMatchEvent>
    {
    }
}
