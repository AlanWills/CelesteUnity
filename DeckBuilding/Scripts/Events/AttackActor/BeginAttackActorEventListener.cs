using Celeste.Events;
using UnityEngine;

namespace Celeste.DeckBuilding.Events
{
    [AddComponentMenu("Celeste/Events/Deck Building/Begin Attack Actor Event Listener")]
    public class BeginAttackActorEventListener : ParameterisedEventListener<BeginAttackActorArgs, BeginAttackActorEvent, BeginAttackActorUnityEvent>
    {
    }
}
