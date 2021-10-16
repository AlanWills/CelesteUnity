using Celeste.Events;
using UnityEngine;

namespace Celeste.DeckBuilding.Events
{
    [AddComponentMenu("Celeste/Deck Building/Events/Card Runtime Event Raiser")]
    public class CardRuntimeEventRaiser : ParameterisedEventRaiser<CardRuntime, CardRuntimeEvent>
    {
    }
}
