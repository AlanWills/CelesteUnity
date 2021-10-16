using Celeste.Events;
using UnityEngine;

namespace Celeste.DeckBuilding.Events
{
    [AddComponentMenu("Celeste/Deck Building/Events/Card Runtime Event Listener")]
    public class CardRuntimeEventListener : ParameterisedEventListener<CardRuntime, CardRuntimeEvent, CardRuntimeUnityEvent>
    {
    }
}
