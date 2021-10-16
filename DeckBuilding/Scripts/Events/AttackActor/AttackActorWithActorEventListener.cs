using Celeste.Events;
using UnityEngine;

namespace Celeste.DeckBuilding.Events
{
    [AddComponentMenu("Celeste/Events/Deck Building/Attack Actor With Actor Event Listener")]
    public class AttackActorWithActorEventListener : ParameterisedEventListener<AttackActorWithActorArgs, AttackActorWithActorEvent, AttackActorWithActorUnityEvent>
    {
    }
}
