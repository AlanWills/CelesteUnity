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
	[CreateAssetMenu(fileName = nameof(DrawCardsFromDeckEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Deck Building/Draw Cards From Deck", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class DrawCardsFromDeckEvent : ParameterisedEvent<DrawCardsFromDeckArgs>
	{
	}
}
