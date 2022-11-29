using Celeste.DeckBuilding.Decks;
using System;
using System.Collections.Generic;
 
namespace Celeste.DeckBuilding.Persistence
{
    [Serializable]
    public class DeckDTO
    {
        public List<int> cardsInDrawPile = new List<int>();
        public List<int> cardsInDiscardPile = new List<int>();
        public List<int> cardsInRemovedPile = new List<int>();

        public DeckDTO(Deck deck)
        {
            cardsInDrawPile.Capacity = deck.NumCardsInDrawPile;
            cardsInDiscardPile.Capacity = deck.NumCardsInDiscardPile;
            cardsInRemovedPile.Capacity = deck.NumCardsInRemovedPile;

            for (int i = 0, n = deck.NumCardsInDrawPile; i < n; ++i)
            {
                cardsInDrawPile.Add(deck.GetCardInDrawPile(i).CardGuid);
            }

            for (int i = 0, n = deck.NumCardsInDiscardPile; i < n; ++i)
            {
                cardsInDiscardPile.Add(deck.GetCardInDiscardPile(i).CardGuid);
            }

            for (int i = 0, n = deck.NumCardsInRemovedPile; i < n; ++i)
            {
                cardsInRemovedPile.Add(deck.GetCardInRemovedPile(i).CardGuid);
            }
        }
    }
}