using Celeste.Events;
using UnityEngine;

namespace Celeste.DeckBuilding.Events
{
    [AddComponentMenu("Celeste/Events/Deck Building/Begin Attack Actor Event Raiser")]
    public class BeginAttackActorEventRaiser : ParameterisedEventRaiser<BeginAttackActorArgs, BeginAttackActorEvent>
    {
    }
}
