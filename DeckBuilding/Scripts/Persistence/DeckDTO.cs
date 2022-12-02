using Celeste.DeckBuilding.Decks;
using System;
using System.Collections.Generic;
 
namespace Celeste.DeckBuilding.Persistence
{
    [Serializable]
    public class DeckDTO
    {
        public List<CardRuntimeDTO> cardsInDrawPile = new List<CardRuntimeDTO>();
        public List<CardRuntimeDTO> cardsInDiscardPile = new List<CardRuntimeDTO>();
        public List<CardRuntimeDTO> cardsInRemovedPile = new List<CardRuntimeDTO>();

        public DeckDTO(Deck deck)
        {
            cardsInDrawPile.Capacity = deck.NumCardsInDrawPile;
            cardsInDiscardPile.Capacity = deck.NumCardsInDiscardPile;
            cardsInRemovedPile.Capacity = deck.NumCardsInRemovedPile;

            for (int i = 0, n = deck.NumCardsInDrawPile; i < n; ++i)
            {
                cardsInDrawPile.Add(new CardRuntimeDTO(deck.GetCardInDrawPile(i)));
            }

            for (int i = 0, n = deck.NumCardsInDiscardPile; i < n; ++i)
            {
                cardsInDiscardPile.Add(new CardRuntimeDTO(deck.GetCardInDiscardPile(i)));
            }

            for (int i = 0, n = deck.NumCardsInRemovedPile; i < n; ++i)
            {
                cardsInRemovedPile.Add(new CardRuntimeDTO(deck.GetCardInRemovedPile(i)));
            }
        }
    }
}