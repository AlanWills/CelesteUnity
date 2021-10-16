using Celeste.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public class CardRuntimeUnityEvent : UnityEvent<CardRuntime> { }

    [CreateAssetMenu(fileName = nameof(CardRuntimeEvent), menuName = "Celeste/Events/Deck Building/Card Runtime Event")]
    public class CardRuntimeEvent : ParameterisedEvent<CardRuntime> { }
}