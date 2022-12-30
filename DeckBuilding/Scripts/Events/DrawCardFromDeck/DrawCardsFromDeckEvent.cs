using System;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;
using Celeste.DeckBuilding.Decks;

namespace Celeste.DeckBuilding.Events
{
	[Serializable]
	public struct DrawCardsFromDeckArgs
	{
		public Deck deck;
		public int quantity;
	}

	[Serializable]
	public class DrawCardsFromDeckUnityEvent : UnityEvent<DrawCardsFromDeckArgs> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(DrawCardsFromDeckEvent), menuName = "Celeste/Events/Deck Building/Draw Cards From Deck")]
	public class DrawCardsFromDeckEvent : ParameterisedEvent<DrawCardsFromDeckArgs>
	{
	}
}
