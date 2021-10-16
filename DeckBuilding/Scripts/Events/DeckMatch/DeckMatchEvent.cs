using Celeste.DeckBuilding.Match;
using Celeste.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public class DeckMatchUnityEvent : UnityEvent<DeckMatch> { }

    [CreateAssetMenu(fileName = nameof(DeckMatchEvent), menuName = "Celeste/Events/Deck Building/Deck Match Event")]
    public class DeckMatchEvent : ParameterisedEvent<DeckMatch> { }
}