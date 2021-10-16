using Celeste.Events;
using UnityEngine;

namespace Celeste.DeckBuilding.Events
{
    [AddComponentMenu("Celeste/Deck Building/Events/Use Card On Actor Event Raiser")]
    public class UseCardOnActorEventRaiser : ParameterisedEventRaiser<CardRuntime, CardRuntimeEvent>
    {
    }
}
