using Celeste.Events;
using UnityEngine;

namespace Celeste.DeckBuilding.Events
{
    [AddComponentMenu("Celeste/Deck Building/Events/Use Card On Actor Event Listener")]
    public class UseCardOnActorEventListener : ParameterisedEventListener<UseCardOnActorArgs, UseCardOnActorEvent, UseCardOnActorUnityEvent>
    {
    }
}
