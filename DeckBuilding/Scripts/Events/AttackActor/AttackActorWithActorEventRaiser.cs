using Celeste.Events;
using UnityEngine;

namespace Celeste.DeckBuilding.Events
{
    [AddComponentMenu("Celeste/Events/Deck Building/Attack Actor With Actor Event Raiser")]
    public class AttackActorWithActorEventRaiser : ParameterisedEventRaiser<AttackActorWithActorArgs, AttackActorWithActorEvent>
    {
    }
}
